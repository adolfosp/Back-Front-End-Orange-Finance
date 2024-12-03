namespace OrangeFinance.Adapters.Configuration;

public class ExchangeConfiguration
{
    public string ExchangeName { get; set; }
    public string ExchangeType { get; set; } = "direct";
    public bool Durable { get; set; } = true;
    public bool AutoDelete { get; set; } = false;
    public Dictionary<string, object> Arguments { get; set; } = [];
    public List<QueueConfiguration> Queues { get; set; } = [];
}
