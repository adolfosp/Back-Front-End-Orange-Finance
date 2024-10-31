using Asp.Versioning;
using Asp.Versioning.Builder;

using GraphQL.Server.Ui.Playground;
using GraphQL.Types;

using OrangeFinance.Endpoints;

using Scalar.AspNetCore;

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

            app.UseSwagger(options =>
            {
                options.RouteTemplate = "openapi/{documentName}.json";
            });

            app.MapScalarApiReference(options =>
            {
                options
                    .WithTitle("My custom API")
                    .WithTheme(ScalarTheme.Alternate)
                    .WithSidebar(true)
                    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
                    .WithPreferredScheme("ApiKey")
                    .WithApiKeyAuthentication(x => x.Token = "my-api-key");
            });
            #region Exemplo antigo do swagger
            //               .UseSwaggerUI(options =>
            //               {
            //#warning "Código de exemplo para adicionar a documentação de todas as versões da API. Contém erro não solucionado porque aparece apenas a v1"
            //                   /* 
            //                        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            //                       IReadOnlyList<ApiVersionDescription> apiVersionDescriptions = provider.ApiVersionDescriptions;

            //                       foreach (ApiVersionDescription apiVersionDescription in apiVersionDescriptions)
            //                       {
            //                           options.SwaggerEndpoint($"/swagger/{apiVersionDescription.GroupName}/swagger.json", apiVersionDescription.GroupName.ToUpperInvariant());

            //                       }
            //                    */


            //                   options.SwaggerEndpoint($"/swagger/v1/swagger.json", "V1");
            //                   options.SwaggerEndpoint($"/swagger/v2/swagger.json", "V2");


            //                   options.DocExpansion(DocExpansion.List);

            //               });
            #endregion

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
                             PlaygroundSettings = new Dictionary<string, object>
                             {
                                 ["editor.theme"] = "light",
                                 ["editor.cursorShape"] = "line",
                             },
                             GraphQLEndPoint = "/graphql",
                             SubscriptionsEndPoint = "/graphql",
                         });

    }

    public static void RegisterApiVersion(this WebApplication app)
    {
        ApiVersionSet apiVersion = app.NewApiVersionSet()
                              .HasApiVersion(new ApiVersion(1, 0))
                              .HasApiVersion(new ApiVersion(2, 0))
                              .ReportApiVersions()
                              .Build();

        RouteGroupBuilder routeGroupBuilder = app.MapGroup("api/v{apiVersion:apiVersion}")
                                                 .WithApiVersionSet(apiVersion);

        routeGroupBuilder.RegisterFarmEndpoints();
        routeGroupBuilder.RegisterSecurityEndpoints();

    }
}
