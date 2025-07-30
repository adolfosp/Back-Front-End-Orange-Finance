using Asp.Versioning;
using Asp.Versioning.Builder;

using GraphQL.Server.Ui.Playground;
using GraphQL.Types;

using OrangeFinance.Endpoints;

using Scalar.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace OrangeFinance.Extensions;

public static class Configuration
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {

        builder.AddLogConfiguration();

        builder.AddOpenTelemetry(builder.Configuration);

        builder.Services.AddHttpContextAccessor();

        builder.AddApiVersion();

    }

    public static void RegisterMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.MapScalarApiReference((options, context) =>
            {
                options
                    .WithTheme(ScalarTheme.Alternate)
                    .WithLayout(ScalarLayout.Classic)
                    .WithFavicon("https://scalar.com/logo-light.svg")
                    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            // Configuração do Swagger UI
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/v1/swagger.json", "V1");
                options.SwaggerEndpoint($"/swagger/v2/swagger.json", "V2");

                options.DocExpansion(DocExpansion.List);  // Exibe as versões de forma expandida
            });
        }
        ;

        app.UseExceptionHandler("/error");

        app.UseHttpsRedirection();
    }

    public static void RegisterGraphQL(this WebApplication app)
    {
        app.UseGraphQL<ISchema>();
        if (app.Environment.IsDevelopment())
        {
            app.UseGraphQLPlayground("/",
                             new PlaygroundOptions
                             {
                                 PlaygroundSettings = new Dictionary<string, object>
                                 {
                                     ["editor.theme"] = "light",
                                     ["editor.cursorShape"] = "line",
                                 },
                                 GraphQLEndPoint = "/graphql",
                                 SubscriptionsEndPoint = "/graphql",
                             });
        }
    }

    public static void RegisterApiVersion(this WebApplication app)
    {
        ApiVersionSet apiVersion = app.NewApiVersionSet()
                              .HasApiVersion(new ApiVersion(1))
                              .HasApiVersion(new ApiVersion(2))
                              .ReportApiVersions()
                              .Build();

        RouteGroupBuilder routeGroupBuilder = app.MapGroup("api/v{apiVersion:apiVersion}")
                                                 .WithApiVersionSet(apiVersion);

        routeGroupBuilder.RegisterFarmEndpoints();
        routeGroupBuilder.RegisterSecurityEndpoints();
        routeGroupBuilder.RegisterInfoEndpoints();
        routeGroupBuilder.RegisterHarvestEndpoints();

    }
}
