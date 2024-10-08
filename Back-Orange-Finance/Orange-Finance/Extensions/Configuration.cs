using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;

using GraphQL.Server.Ui.Playground;
using GraphQL.Types;

using Microsoft.OpenApi.Models;

using OrangeFinance.Endpoints;

using Scalar.AspNetCore;

namespace OrangeFinance.Extensions;

public static class Configuration
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {

        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new HeaderApiVersionReader("X-ApiVersion"),
                new UrlSegmentApiVersionReader());

        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
               {
                   var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                   foreach (var description in provider.ApiVersionDescriptions)
                   {
                       Console.WriteLine($"Found API version: {description.GroupName}");

                       if (!options.SwaggerGeneratorOptions.SwaggerDocs.ContainsKey(description.GroupName))
                       {
                           options.SwaggerDoc(description.GroupName, new OpenApiInfo
                           {
                               Title = $"Minha API {description.ApiVersion}",
                               Version = description.ApiVersion.ToString(),
                               Description = $"Documentação da API para a versão {description.ApiVersion}"
                           });
                       }

                   }
               });

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
