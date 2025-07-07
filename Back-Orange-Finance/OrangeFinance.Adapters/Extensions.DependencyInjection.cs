using Microsoft.Extensions.DependencyInjection;
using OrangeFinance.Adapters.Configuration;
using OrangeFinance.Adapters.Rpc;
using OrangeFinance.Adapters.Serialization;
using RabbitMQ.Client;
using System.Diagnostics;

namespace OrangeFinance.Adapters;

public static partial class Extensions
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, Action<RabbitMQConfigurationBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(action);
        RabbitMQConfigurationBuilder builder = new(services);

        action(builder);

        builder.Build();

        return services;
    }


    public static IServiceCollection AddAmqpRpcClient(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddTransient(sp => sp.GetRequiredService<IConnection>().CreateModel());

        services.AddScoped(sp => new AmqpRpc(
                 sp.GetRequiredService<IModel>(),
                 sp.GetRequiredService<IAmqpSerializer>(),
                 sp.GetRequiredService<ActivitySource>())
         );

        return services;
    }
}
