using Humanizer;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OrangeFinance.Adapters.Serialization;

using Polly;

using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace OrangeFinance.Adapters.Configuration;

public sealed class RabbitMQConfigurationBuilder
{
    private readonly IServiceCollection _services;
    private IConfiguration _configuration;
    private string _configurationPrefix = "RABBITMQ";
    private int _connectMaxAttempts = 8;
    private Func<int, TimeSpan> _produceWaitConnectWait = (retryAttempt) => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
    private List<ExchangeConfiguration> _exchanges = [];

    public RabbitMQConfigurationBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public RabbitMQConfigurationBuilder WithSerializer<T>() where T : class, IAmqpSerializer
    {
        _services.AddSingleton<IAmqpSerializer, T>();
        return this;
    }

    public RabbitMQConfigurationBuilder WithConfiguration(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        this._configuration = configuration;
        return this;
    }

    public RabbitMQConfigurationBuilder WithConfigurationPrefix(string prefix)
    {
        ArgumentNullException.ThrowIfNull(prefix);
        this._configurationPrefix = prefix;
        return this;
    }

    public RabbitMQConfigurationBuilder WithConnectionMaxAttempts(int connectionMaxAttempts, Func<int, TimeSpan> produceWaitConnectWait = null)
    {
        if (connectionMaxAttempts < 0) throw new ArgumentOutOfRangeException(nameof(connectionMaxAttempts), "ConnectMaxAttempts must bem greater or equal zero.");
        ArgumentNullException.ThrowIfNull(produceWaitConnectWait);

        this._connectMaxAttempts = connectionMaxAttempts;
        if (produceWaitConnectWait != null)
        {
            this._produceWaitConnectWait = produceWaitConnectWait;
        }
        return this;
    }

    public RabbitMQConfigurationBuilder WithExchange(ExchangeConfiguration exchangeConfig)
    {
        ArgumentNullException.ThrowIfNull(exchangeConfig);
        _exchanges.Add(exchangeConfig);
        return this;
    }

    public void Build()
    {
        ArgumentNullException.ThrowIfNull(this._configuration);

        this._services.AddTransient(sp => sp.GetRequiredService<IConnection>().CreateModel());


        this._services.AddSingleton(sp =>
        {
            ConnectionFactory factory = new();
            this._configuration.Bind(this._configurationPrefix, factory);
            return factory;
        });

        this._services.AddSingleton(sp => Policy
               .Handle<BrokerUnreachableException>()
               .WaitAndRetry(this._connectMaxAttempts, retryAttempt =>
               {
                   TimeSpan wait = this._produceWaitConnectWait(retryAttempt);
                   Console.WriteLine($"Can't create a connection with RabbitMQ. We wil try again in {wait.Humanize()}.");
                   return wait;
               })
               .Execute(() =>
               {
                   System.Diagnostics.Debug.WriteLine("Trying to create a connection with RabbitMQ");

                   IConnection connection = sp.GetRequiredService<ConnectionFactory>().CreateConnection();

                   Console.WriteLine(@$"Connected on RabbitMQ '{connection}' with name '{connection.ClientProvidedName}'. 
                                    ....Local Port: {connection.LocalPort}
                                    ....Remote Port: {connection.RemotePort}
                                    ....cluster_name: {connection.ServerProperties.AsString("cluster_name")}
                                    ....copyright: {connection.ServerProperties.AsString("copyright")}
                                    ....information: {connection.ServerProperties.AsString("information")}
                                    ....platform: {connection.ServerProperties.AsString("platform")}
                                    ....product: {connection.ServerProperties.AsString("product")}
                                    ....version: {connection.ServerProperties.AsString("version")}");


                   var channel = connection.CreateModel();

                   foreach (var exchangeConfig in _exchanges)
                   {
                       channel.ExchangeDeclare(
                           exchangeConfig.ExchangeName,
                           exchangeConfig.ExchangeType,
                           exchangeConfig.Durable,
                           exchangeConfig.AutoDelete,
                           exchangeConfig.Arguments
                       );
                       Console.WriteLine($"Exchange '{exchangeConfig.ExchangeName}' created with type '{exchangeConfig.ExchangeType}'.");

                       foreach (var queueConfig in exchangeConfig.Queues)
                           BindQueueToExchange(channel, exchangeConfig, queueConfig);

                   }

                   return connection;
               })
       );
    }

    private static void BindQueueToExchange(IModel channel, ExchangeConfiguration exchangeConfig, QueueConfiguration queueConfig)
    {
        channel.QueueDeclare(queue: queueConfig.QueueName,
                             durable: queueConfig.Durable,
                             exclusive: false,
                             autoDelete: queueConfig.AutoDelete,
                             arguments: queueConfig.Arguments);
        Console.WriteLine($"Queue '{queueConfig.QueueName}' created.");

        channel.QueueBind(
            queueConfig.QueueName,
            exchangeConfig.ExchangeName,
            queueConfig.QueueName
        );
        Console.WriteLine($"Queue '{queueConfig.QueueName}' binded to exchange '{exchangeConfig.ExchangeName}'.");

    }
}