namespace OrangeFinance.Adapters.Configuration;

public class QueueConfiguration
{
    public string QueueName { get; set; }
    public bool Durable { get; set; }
    public bool AutoDelete { get; set; }
    public Dictionary<string, object> Arguments { get; set; } = [];

}
