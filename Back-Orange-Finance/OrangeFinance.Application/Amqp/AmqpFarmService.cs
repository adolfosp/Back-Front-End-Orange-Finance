using OrangeFinance.Adapters.Rpc;

namespace OrangeFinance.Application.Amqp;

/// <summary>
/// Filas definidas na classe ExchangeQueueConfigurationFactory
/// </summary>
/// <param name="amqpRpc"></param>
public sealed class AmqpFarmService(AmqpRpc amqpRpc) : AmqpServiceBase
{
    private readonly AmqpRpc _amqpRpc = amqpRpc;

    protected override string ExchangeName => "farm_service";

    protected override string ServiceName => "FarmService";

    public void SendLocationFarm<TRequest>(TRequest requestModel, string routeName = "")
    {
        //Quando você publica, não devemos saber da fila e apenas da exchange
        _amqpRpc.Send(ExchangeName, routeName, requestModel);
    }

    public TRequest ReceiveLocationFarm<TRequest>()
    {
        return _amqpRpc.Receive<TRequest>("location_farm", TimeSpan.FromSeconds(15));
    }

}
