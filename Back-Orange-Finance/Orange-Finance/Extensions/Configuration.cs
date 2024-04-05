using GraphQL.Server.Ui.Playground;
using GraphQL.Types;

namespace OrangeFinance.Extensions;

public static class Configuration
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services
               .AddEndpointsApiExplorer()
               .AddSwaggerGen();
    }

    public static void RegisterMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger()
               .UseSwaggerUI();
        }
        app.UseExceptionHandler("/error");

        app.UseHttpsRedirection();
    }

    public static void RegisterGraphQL(this WebApplication app)
    {
        app.UseGraphQL<ISchema>();
        app.UseGraphQLPlayground("/",
                         new PlaygroundOptions
                         {
                             GraphQLEndPoint = "/graphql",
                             SubscriptionsEndPoint = "/graphql",
                         });

    }
}
