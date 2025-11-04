using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace OrangeFinance.Extensions;

internal static class OpenTelemetry
{
    public static void AddOpenTelemetry(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        var serviceName = builder.Configuration["OpenTelemetry:ServiceName"] ?? "OrangeFinanceApi";

        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(serviceName);

        builder.Logging.AddOpenTelemetry(x =>
        {
            x.IncludeScopes = true;
            x.IncludeFormattedMessage = true;
        });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(opts =>
            {
                opts.AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                    .AddMeter("Microsoft.AspNetCore.Http.Connections")
                    .AddMeter("Microsoft.AspNetCore.Routing")
                    .AddMeter("Microsoft.AspNetCore.Diagnostics")
                    .AddMeter("Microsoft.AspNetCore.RateLimiting")
                    .AddRuntimeInstrumentation()
                    .SetResourceBuilder(resourceBuilder)
                    .AddMeter(serviceName);
            })
            .WithTracing(opts =>
            {
                opts.SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation(options =>
                        {
                            options.Filter = httpContext => true;
                            options.EnrichWithHttpRequest = (activity, httpRequest) =>
                            {
                                foreach (var (key, value) in httpRequest.Query)
                                {
                                    activity.SetTag($"http.request.query.{key}", value.ToString());
                                }

                                if (httpRequest.Headers.TryGetValue("User-Agent", out var userAgent))
                                {
                                    activity.SetTag("http.request.header.user_agent", userAgent.ToString());
                                }

                                if (httpRequest.Headers.TryGetValue("Custom-Header", out var customHeader))
                                {
                                    activity.SetTag("http.request.header.custom_header", customHeader.ToString());
                                }
                            };
                        }
                    )
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(opts =>
                    {

#if DEBUG
                        Console.WriteLine("Modo Debug: OTLP.");
                        var urlOTLPAspire = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT");
                        opts.Endpoint = new Uri(urlOTLPAspire!);
#else
                        Console.WriteLine("Modo Produção: OTLP.");

                        opts.Endpoint = new Uri("http://localhost:4317");

#endif


                    });
            });
    }
}