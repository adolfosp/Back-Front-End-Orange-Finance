using OrangeFinance.Adapters.Rpc;

namespace OrangeFinance.Application.Amqp;

public sealed class AmqpUserService : AmqpServiceBase
{
    private readonly AmqpRpc _amqpRpc;

    public AmqpUserService(AmqpRpc amqpRpc)
    {
        _amqpRpc = amqpRpc;
    }

    protected override string ExchangeName => "user_service";

    protected override string ServiceName => "UserService";

    public void SendUser<TRequest>(string exchangeName, string routingKey, TRequest requestModel)
    {
        _amqpRpc.FireAndForget(exchangeName, routingKey, requestModel);
    }
}
