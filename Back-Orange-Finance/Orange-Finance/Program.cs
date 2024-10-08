using OrangeFinance;
using OrangeFinance.Application;
using OrangeFinance.Common.Mapping.MongoDB;
using OrangeFinance.Extensions;
using OrangeFinance.Infrastructure;
using OrangeFinance.Infrastructure.Persistence.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddProblemDetails();

builder.AddJwtAuthentication();
builder.AddClientsFactory();
builder.RegisterServices();

MongoDBMappingConfig.RegisterMappings();

/*App*/

var app = builder.Build();
app.RegisterMiddlewares();

app.EnsureCreatedDatabase();
app.RegisterGraphQL();
app.RegisterApiVersion();


app.Run();