using Microsoft.AspNetCore.Mvc.Infrastructure;

using OrangeFinance.Common;

namespace OrangeFinance;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddSingleton<ProblemDetailsFactory, OrangeFinanceProblemDetailsFactory>();
        return services;
    }
}
