using OrangeFinance.Adapters.Rpc;

namespace OrangeFinance.Application.Amqp;

public sealed class AmqpFarmService(AmqpRpc amqpRpc) : AmqpServiceBase
{
    private readonly AmqpRpc _amqpRpc = amqpRpc;

    protected override string ExchangeName => "farm_service";

    protected override string ServiceName => "FarmService";

    public void SendLocationFarm<TRequest>(TRequest requestModel, string routeName = "")
    {
        _amqpRpc.Send(ExchangeName, routeName, requestModel);
    }
}
