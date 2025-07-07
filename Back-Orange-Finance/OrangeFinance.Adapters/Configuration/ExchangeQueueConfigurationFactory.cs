namespace OrangeFinance.Adapters.Configuration;

public static class ExchangeQueueConfigurationFactory
{

    public static ExchangeConfiguration CreateFarmExchangeConfiguration()
    {
        return new ExchangeConfiguration
        {
            ExchangeName = "farm_service",
            ExchangeType = "direct",
            Durable = true,
            AutoDelete = false,
            Arguments = [],
            Queues = [
                new() {
                        QueueName = "location_farm",
                        Durable = true,
                        AutoDelete = false,
                        Arguments = []
                      }
                    ]
        };
    }
}
