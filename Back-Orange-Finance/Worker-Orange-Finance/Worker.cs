using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Worker_Orange_Finance;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;

        ConnectionFactory factory = new();
        configuration.Bind("RabbitMQ", factory);

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: "farm_service", type: ExchangeType.Direct, durable: true);
        _channel.QueueDeclare(queue: "location_farm", durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queue: "location_farm", exchange: "farm_service", routingKey: "location_farm");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation("Mensagem recebida da fila location_farm: {message}", message);

            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(queue: "location_farm", autoAck: false, consumer: consumer);

    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
