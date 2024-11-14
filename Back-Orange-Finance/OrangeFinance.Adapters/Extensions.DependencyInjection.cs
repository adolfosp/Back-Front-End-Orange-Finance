using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

using OrangeFinance.Adapters.Rpc;
using OrangeFinance.Adapters.Serialization;

using RabbitMQ.Client;

namespace OrangeFinance.Adapters;

internal static partial class Extensions
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, Action<RabbitMQConfigurationBuilder> action)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));
        if (action is null) throw new ArgumentNullException(nameof(action));
        RabbitMQConfigurationBuilder builder = new RabbitMQConfigurationBuilder(services);

        action(builder);

        builder.Build();

        return services;
    }


    public static IServiceCollection AddAmqpRpcClient(this IServiceCollection services)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));
        services.AddTransient(sp => sp.GetRequiredService<IConnection>().CreateModel());

        services.AddScoped(sp => new AmqpRpc(
                 sp.GetRequiredService<IModel>(),
                 sp.GetRequiredService<IAmqpSerializer>(),
                 sp.GetRequiredService<ActivitySource>(),
                 TimeSpan.FromMinutes(5)
             )
         );

        return services;
    }
}
