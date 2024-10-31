using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

using Microsoft.OpenApi.Models;

namespace OrangeFinance.Extensions;

internal static class ApiVersionConfiguration
{
    public static void AddApiVersion(this WebApplicationBuilder builder)
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
}
