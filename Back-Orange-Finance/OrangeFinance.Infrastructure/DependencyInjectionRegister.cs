using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OrangeFinance.Application.Common.Interfaces.Persistence.DomainEvents;
using OrangeFinance.Application.Common.Interfaces.Persistence.Farms;
using OrangeFinance.Infrastructure.Persistence;
using OrangeFinance.Infrastructure.Persistence.Interceptors;
using OrangeFinance.Infrastructure.Persistence.Models;
using OrangeFinance.Infrastructure.Repositories;

namespace OrangeFinance.Infrastructure;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configration)
    {
        var mongoSettings = new MongoDBSettings();
        configration.Bind(MongoDBSettings.SectionName, mongoSettings);

        services.AddDbContext<OrangeFinanceDbContext>(options =>
            options.UseSqlServer("Server=sqlserver,1433;Trusted_Connection=false;Encrypt=False;Database=Orange_Finance_Web;User Id=sa;Password=SqlServer2022!"));

        services.AddSingleton(serviceProvider => new MongoDBContext(mongoSettings.ConnectionString, mongoSettings.DatabaseName));

        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddScoped<IReadFarmRepository, FarmRepository>();
        services.AddScoped<IWriteFarmRepository, FarmRepository>();
        services.AddScoped<IWriteDomainEventsRepository, DomainEventsRepository>();

        return services;
    }

}