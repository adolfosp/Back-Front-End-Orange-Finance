using Serilog;
using Serilog.Context;

namespace OrangeFinance.Extensions;

internal static class LogConfiguration
{
    public static void AddLogConfiguration(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });
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
