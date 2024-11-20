﻿using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

using OrangeFinance.Adapters.Configuration;
using OrangeFinance.Adapters.Rpc;
using OrangeFinance.Adapters.Serialization;

using RabbitMQ.Client;

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
        if (services is null) throw new ArgumentNullException(nameof(services));
        services.AddTransient(sp => sp.GetRequiredService<IConnection>().CreateModel());

        services.AddScoped(sp => new AmqpRpc(
                 sp.GetRequiredService<IModel>(),
                 sp.GetRequiredService<IAmqpSerializer>(),
                 sp.GetRequiredService<ActivitySource>())
         );

        return services;
    }
}