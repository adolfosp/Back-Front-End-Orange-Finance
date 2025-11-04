using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrangeFinance.Adapters;
using OrangeFinance.Adapters.Configuration;
using OrangeFinance.Adapters.Serialization;
using OrangeFinance.Application.Common.Interfaces;
using OrangeFinance.Application.Common.Interfaces.Persistence;
using OrangeFinance.Application.Common.Interfaces.Persistence.DomainEvents;
using OrangeFinance.Application.Common.Interfaces.Persistence.Farms;
using OrangeFinance.Application.Common.Interfaces.Persistence.Finances;
using OrangeFinance.Application.Common.Interfaces.Persistence.Harvests;
using OrangeFinance.Infrastructure.Persistence;
using OrangeFinance.Infrastructure.Persistence.Interceptors;
using OrangeFinance.Infrastructure.Persistence.Settings;
using OrangeFinance.Infrastructure.Repositories;

namespace OrangeFinance.Infrastructure;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var mongoSettings = new MongoDBSettings();
        builder.Configuration.Bind(MongoDBSettings.SectionName, mongoSettings);

#if DEBUG
        Console.WriteLine("--Modo Debug: Redis.");
        var redisConnection = builder.Configuration.GetConnectionString("cache");
        if (string.IsNullOrEmpty(redisConnection))
            redisConnection = redisConnection = builder.Configuration.GetSection("RedisSettings")
                                                                     .GetValue<string>("ConnectionString");

        var redisSettings = new RedisSettings
        {
            ConnectionString = redisConnection!
        };

#else
        Console.WriteLine("--Modo Produção: Redis.");

        var redisSettings = new RedisSettings();
        builder.Configuration.Bind(RedisSettings.SectionName, redisSettings);
#endif

#if DEBUG
        Console.WriteLine("--Modo Debug: Postgres.");

        var urlPostgresAspire = builder.Configuration.GetConnectionString("mainDB");

        if (string.IsNullOrEmpty(urlPostgresAspire))
            urlPostgresAspire = builder.Configuration.GetSection(PostgresSettings.SectionName).GetValue<string>("ConnectionString"); ;

        services.AddDbContext<OrangeFinanceDbContext>(options =>
            options.UseNpgsql(urlPostgresAspire));
#else
        Console.WriteLine("--Modo Produção: Postgres.");

        var postgresSettings = new PostgresSettings();
        builder.Configuration.Bind(PostgresSettings.SectionName, postgresSettings);
        services.AddDbContext<OrangeFinanceDbContext>(options =>
                options.UseNpgsql(postgresSettings.ConnectionString));
#endif

        Console.WriteLine("--Comecando o MongoDB");
        services.AddSingleton(serviceProvider => new MongoDBContext(mongoSettings.ConnectionString, mongoSettings.DatabaseName));
        services.AddSingleton(new RedisDBContext(redisSettings.ConnectionString));
        services.AddSingleton<ICacheRepository, CacheRepository>();

        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddScoped<IReadFarmRepository, FarmRepository>();
        services.AddScoped<IWriteFarmRepository, FarmRepository>();
        services.AddScoped<IWriteDomainEventsRepository, DomainEventsRepository>();

        services.AddScoped<IWriteHarvestRepository, HarvestRepository>();
        services.AddScoped<IWriteFinanceRepository, FinanceRepository>();

        services.AddScoped<IUnitOfWork>(c => c.GetRequiredService<OrangeFinanceDbContext>());

        #region RabbitMQ

        Console.WriteLine("--Comecando o RabbitMQ");
        services.AddRabbitMQ(cfg => cfg.WithExchange(ExchangeQueueConfigurationFactory.CreateFarmExchangeConfiguration()).WithConfiguration(builder.Configuration)
                                       .WithSerializer<SystemTextJsonAmqpSerializer>());




        services.AddAmqpRpcClient();
        #endregion

        Console.WriteLine("--Finalizando Infra");

        return services;
    }

}