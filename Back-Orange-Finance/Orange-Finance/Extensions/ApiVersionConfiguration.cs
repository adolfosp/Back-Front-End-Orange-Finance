using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace OrangeFinance.Extensions;

internal static class ApiVersionConfiguration
{
    public static void AddApiVersion(this WebApplicationBuilder builder)
    {
        #region Configurando Versões da API
        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new HeaderApiVersionReader("X-ApiVersion"),
                new UrlSegmentApiVersionReader());
        })
          .AddApiExplorer(options =>
          {
              options.GroupNameFormat = "'v'VVV";
              options.SubstituteApiVersionInUrl = true;
          });
        #endregion

        #region Configurando para o SCALAR entender que há versões da API
        var versions = new List<ApiVersion> { new(1), new(2) };

        foreach (var version in versions)
        {
            builder.Services.Configure<ScalarOptions>(options => options.AddDocument($"v{version.MajorVersion}", $"v{version.MajorVersion}"));
            builder.Services.AddOpenApi($"v{version.MajorVersion}", options =>
            {
                options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;

                options.AddDocumentTransformer((document, context, ct) =>
                {
                    var provider = context.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                    var description = provider.ApiVersionDescriptions.FirstOrDefault(d => d.GroupName == context.DocumentName);

                    document.Info = new OpenApiInfo
                    {
                        Title = "Orange-Finance",
                        Version = description?.ApiVersion.ToString() ?? context.DocumentName,
                        Description = description?.IsDeprecated == true ? "This API version is deprecated." : null
                    };

                    return Task.CompletedTask;
                });
            });
        }
        #endregion

        // O método AddOpenApi() é utilizado para configurar a integração com ferramentas de documentação de API como o Swagger e outras ferramentas que geram documentação baseada no padrão OpenAPI
        builder.Services.AddOpenApi();

        // O método AddEndpointsApiExplorer() é responsável por adicionar suporte para explorar os endpoints (rotas) da API. No contexto de Minimal APIs, esse método permite que o sistema de documentação (como o Swagger) descubra e registre todos os endpoints definidos na aplicação.
        builder.Services.AddEndpointsApiExplorer();

        // O método AddSwaggerGen() configura a geração de documentação Swagger para a API. Ele habilita a funcionalidade de geração do arquivo swagger.json, que descreve a estrutura da API, os endpoints, os parâmetros e outros aspectos importantes da API. Esse arquivo swagger.json é consumido pelo Swagger UI para renderizar uma interface gráfica interativa.
        builder.Services.AddSwaggerGen(options =>
        {
            var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            // Gerar o Swagger Doc para cada versão da API
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = $"My API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                    Description = $"Documentation for API version {description.ApiVersion}"
                });
            }
        });

    }
}
