using OrangeFinance;
using OrangeFinance.Application;
using OrangeFinance.Common.Mapping.MongoDB;
using OrangeFinance.Extensions;
using OrangeFinance.Infrastructure;
using OrangeFinance.Infrastructure.Persistence.Configurations;

using Serilog;

using SerilogTracing;

using var _ = new ActivityListenerConfiguration()
    .Instrument.AspNetCoreRequests(opts =>
    {
        opts.IncomingTraceParent = IncomingTraceParent.Trust;
    })
    .TraceToSharedLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddPresentation();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddProblemDetails();

    builder.AddJwtAuthentication();
    builder.AddClientsFactory();
    builder.RegisterServices();

    Log.Information("Starting up application");

    MongoDBMappingConfig.RegisterMappings();

    /*App*/

    var app = builder.Build();
    app.RegisterMiddlewares();

    app.EnsureCreatedDatabase();
    app.RegisterGraphQL();
    app.RegisterApiVersion();
    app.RegisterLogConfiguration();

    app.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
    return 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}