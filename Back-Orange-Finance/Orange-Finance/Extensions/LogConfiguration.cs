using Serilog;
using Serilog.Context;

namespace OrangeFinance.Extensions;

internal static class LogConfiguration
{
    public static void AddLogConfiguration(this WebApplicationBuilder builder)
    {
#if DEBUG
        Console.WriteLine("--Modo Debug: UseSerilog.");
        var seqConnectionString = builder.Configuration.GetConnectionString("seq");

        if (string.IsNullOrEmpty(seqConnectionString))
        {
            Console.WriteLine("---Modo Docker-compose: UseSerilog.");

            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });
        }
        else
        {
            Console.WriteLine("---Modo ASPIRE: UseSerilog.");

            builder.Host.UseSerilog((context, services, configuration) =>
            {
                var urlSeqAspire = builder.Configuration.GetConnectionString("seq");

                configuration.WriteTo.Seq(urlSeqAspire!);

            });

        }
#else
        Console.WriteLine("--Modo Produção: UseSerilog.");
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

#endif

    }

    public static void RegisterLogConfiguration(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.Use(async (context, next) =>
        {
            LogContext.PushProperty("UserId", context.User?.Identity?.Name ?? "anonymous");

            await next.Invoke();
        });
    }
}
