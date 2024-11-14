using System.Collections.Concurrent;
using System.Diagnostics;

using Humanizer;

using OrangeFinance.Adapters.Serialization;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace OrangeFinance.Adapters.Rpc;

public class AmqpRpc
{
    private readonly IModel model;
    private readonly IAmqpSerializer serializer;
    private readonly ActivitySource activitySource;
    private readonly TimeSpan defaultTimeout;

    public AmqpRpc(IModel model, IAmqpSerializer serializer, ActivitySource activitySource, TimeSpan defaultTimeout)
    {
        this.model = model;
        this.serializer = serializer;
        this.activitySource = activitySource;
        this.defaultTimeout = defaultTimeout;
    }

    public void FireAndForget<TRequest>(string exchangeName, string routingKey, TRequest requestModel)
    {
        this.Send(exchangeName, routingKey, requestModel, null);
    }

    protected virtual void Send<TRequest>(string exchangeName, string routingKey, TRequest requestModel, string callbackQueueName = null)
    {
        using Activity currentActivity = this.activitySource.SafeStartActivity("AmqpRpc.Send", ActivityKind.Client);
        currentActivity.AddTag("Exchange", exchangeName);
        currentActivity.AddTag("RoutingKey", routingKey);
        currentActivity.AddTag("CallbackQueue", callbackQueueName);

        IBasicProperties requestProperties = this.model.CreateBasicProperties()
                                                .SetTelemetry(currentActivity)
                                                .SetMessageId()
                                                .SetReplyTo(callbackQueueName);

        currentActivity.AddTag("MessageId", requestProperties.MessageId);
        currentActivity.AddTag("CorrelationId", requestProperties.CorrelationId);

        this.model.BasicPublish(exchangeName, routingKey, requestProperties, this.serializer.Serialize(requestProperties, requestModel));

        currentActivity.SetEndTime(DateTime.UtcNow);
    }

    protected virtual TResponse Receive<TResponse>(QueueDeclareOk queue, TimeSpan receiveTimeout)
    {
        using BlockingCollection<AmqpResponse<TResponse>> localQueue = [];

        EventingBasicConsumer consumer = new(this.model);

        // First Task
        consumer.Received += (sender, receivedItem) =>
        {
            using Activity receiveActivity = this.activitySource.SafeStartActivity("AmqpRpc.Receive", ActivityKind.Server);
            receiveActivity.SetParentId(receivedItem.BasicProperties.GetTraceId(), receivedItem.BasicProperties.GetSpanId(), ActivityTraceFlags.Recorded);
            receiveActivity.AddTag("Queue", queue.QueueName);
            receiveActivity.AddTag("MessageId", receivedItem.BasicProperties.MessageId);
            receiveActivity.AddTag("CorrelationId", receivedItem.BasicProperties.CorrelationId);

            if (receivedItem.BasicProperties.TryReconstructException(out AmqpRpcRemoteException exception))
            {
                localQueue.Add(new AmqpResponse<TResponse>(exception));
                localQueue.CompleteAdding();
            }
            else
            {
                TResponse result = default;
                try
                {
                    result = this.serializer.Deserialize<TResponse>(receivedItem);
                }
                catch (Exception ex)
                {
                    receiveActivity.SetStatus(ActivityStatusCode.Error);
                    receiveActivity.AddEvent(new("Erro na serialização ", tags: new() { { "Exception", ex } }));
                    throw;
                }
                localQueue.Add(new AmqpResponse<TResponse>(result));
                localQueue.CompleteAdding();
            }
            receiveActivity.SetEndTime(DateTime.UtcNow);
        };

        // Second Task
        string consumerTag = this.model.BasicConsume(queue: queue.QueueName, autoAck: true, consumer: consumer);
        AmqpResponse<TResponse> responseModel;
        try
        {
            if (!localQueue.TryTake(out responseModel, receiveTimeout))
            {
                throw new TimeoutException($"The operation has timed-out after {receiveTimeout.Humanize()} waiting a RPC response at {queue.QueueName} queue.");
            }
        }
        finally
        {
            this.model.BasicCancelNoWait(consumerTag);
        }

        return responseModel.Exception != null ? throw responseModel.Exception : responseModel.Result;
    }
}
