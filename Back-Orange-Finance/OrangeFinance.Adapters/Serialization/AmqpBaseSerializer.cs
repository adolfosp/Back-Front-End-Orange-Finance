using System.Diagnostics;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrangeFinance.Adapters.Serialization;

public abstract class AmqpBaseSerializer(ActivitySource activitySource, string name) : IAmqpSerializer
{
    private readonly ActivitySource _activitySource = activitySource;
    private readonly string _className = name;

    protected abstract TResponse DeserializeInternal<TResponse>(IBasicProperties basicProperties, ReadOnlyMemory<byte> body);

    protected abstract byte[] SerializeInternal<T>(IBasicProperties basicProperties, T objectToSerialize);

    public TResponse Deserialize<TResponse>(BasicDeliverEventArgs eventArgs)
    {
        ArgumentNullException.ThrowIfNull(eventArgs);
        if (eventArgs.BasicProperties is null) throw new ArgumentNullException(nameof(eventArgs.BasicProperties));

        using Activity receiveActivity = this._activitySource.SafeStartActivity($"{this._className}.Deserialize", ActivityKind.Internal);
        TResponse returnValue = default;
        try
        {
            returnValue = this.DeserializeInternal<TResponse>(eventArgs.BasicProperties, eventArgs.Body);
        }
        catch (Exception ex)
        {
            receiveActivity?.SetStatus(ActivityStatusCode.Error, ex.ToString());
            throw;
        }
        finally
        {
            receiveActivity?.SetEndTime(DateTime.UtcNow);
        }
        return returnValue;
    }

    public byte[] Serialize<T>(IBasicProperties basicProperties, T objectToSerialize)
    {
        if (basicProperties is null) throw new ArgumentNullException(nameof(basicProperties));

        using Activity receiveActivity = this._activitySource.SafeStartActivity($"{this._className}.Serialize", ActivityKind.Internal);
        byte[] returnValue = default;
        try
        {
            returnValue = this.SerializeInternal(basicProperties, objectToSerialize);
        }
        catch (Exception ex)
        {
            receiveActivity?.SetStatus(ActivityStatusCode.Error, ex.ToString());
            throw;
        }
        finally
        {
            receiveActivity?.SetEndTime(DateTime.UtcNow);
        }
        return returnValue;
    }
}
