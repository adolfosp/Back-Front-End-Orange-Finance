using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrangeFinance.Application.Amqp;
using OrangeFinance.Application.Common.Behaviors;
using OrangeFinance.Application.Harvests;
using OrangeFinance.Application.Security;
using System.Reflection;

namespace OrangeFinance.Application;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjectionRegister).Assembly));

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<SecurityService>();
        services.AddScoped<AmqpFarmService>();
        services.AddScoped<HarvestsAppService>();

        return services;
    }
}