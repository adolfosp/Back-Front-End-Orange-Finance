using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OrangeFinance.Adapters;
using OrangeFinance.Adapters.Configuration;
using OrangeFinance.Adapters.Serialization;
using OrangeFinance.Application.Common.Interfaces;
using OrangeFinance.Application.Common.Interfaces.Persistence.DomainEvents;
using OrangeFinance.Application.Common.Interfaces.Persistence.Farms;
using OrangeFinance.Infrastructure.Persistence;
using OrangeFinance.Infrastructure.Persistence.Interceptors;
using OrangeFinance.Infrastructure.Persistence.Models;
using OrangeFinance.Infrastructure.Repositories;

namespace OrangeFinance.Infrastructure;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoSettings = new MongoDBSettings();
        configuration.Bind(MongoDBSettings.SectionName, mongoSettings);

        var redisSettings = new RedisSettings();
        configuration.Bind(RedisSettings.SectionName, redisSettings);

        var sqlServerSettings = new SqlServerSettings();
        configuration.Bind(SqlServerSettings.SectionName, sqlServerSettings);

        services.AddDbContext<OrangeFinanceDbContext>(options =>
            options.UseSqlServer(sqlServerSettings.ConnectionString));

        services.AddSingleton(serviceProvider => new MongoDBContext(mongoSettings.ConnectionString, mongoSettings.DatabaseName));
        services.AddSingleton(new RedisDBContext(redisSettings.ConnectionString));
        services.AddSingleton<ICacheRepository, CacheRepository>();

        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddScoped<IReadFarmRepository, FarmRepository>();
        services.AddScoped<IWriteFarmRepository, FarmRepository>();
        services.AddScoped<IWriteDomainEventsRepository, DomainEventsRepository>();



        #region RabbitMQ
        services.AddRabbitMQ(cfg => cfg.WithExchange(ExchangeQueueConfigurationFactory.CreateFarmExchangeConfiguration()).WithConfiguration(configuration)
                                       .WithSerializer<SystemTextJsonAmqpSerializer>());

        services.AddAmqpRpcClient();
        #endregion


        return services;
    }

}