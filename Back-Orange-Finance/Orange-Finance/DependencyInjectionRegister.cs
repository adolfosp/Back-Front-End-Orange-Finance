using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.Types;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using OrangeFinance.Common.Errors;
using OrangeFinance.Common.Mapping;
using OrangeFinance.GraphQL.Farms.Mutations;
using OrangeFinance.GraphQL.Farms.Queries;
using OrangeFinance.GraphQL.Farms.Schemas;
using OrangeFinance.GraphQL.Farms.Types;

namespace OrangeFinance;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddSingleton<ProblemDetailsFactory, OrangeFinanceProblemDetailsFactory>();
        services.AddMappings();
        services.AddGraphQL();
        return services;
    }

    private static IServiceCollection AddGraphQL(this IServiceCollection services)
    {
        services.AddGraphQL(options =>
        {
            options.AddSystemTextJson();
            options.UseApolloTracing(true);

        });

        services.AddScoped<FarmInputType>();
        services.AddScoped<FarmQueryGraph>();
        services.AddScoped<FarmMutationGraph>();
        services.AddScoped<ISchema, FarmSchema>(provider => new FarmSchema(new SelfActivatingServiceProvider(provider)));
        return services;
    }
}
