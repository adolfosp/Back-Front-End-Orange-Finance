using Microsoft.AspNetCore.Mvc.Infrastructure;

using OrangeFinance.Common.Erros;
using OrangeFinance.Common.Mapping;

namespace OrangeFinance;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddSingleton<ProblemDetailsFactory, OrangeFinanceProblemDetailsFactory>();
        services.AddMappings();
        return services;
    }
}
