using System.Diagnostics;

namespace OrangeFinance.Application.Amqp;

public abstract class AmqpServiceBase
{
    protected abstract string ExchangeName { get; }

    protected abstract string ServiceName { get; }
    protected virtual void SetTelemetry(Activity activity) { }

}
