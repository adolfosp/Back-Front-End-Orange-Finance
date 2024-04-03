using OrangeFinance;
using OrangeFinance.Application;
using OrangeFinance.Endpoints;
using OrangeFinance.Extensions;
using OrangeFinance.Infrastructure;
using OrangeFinance.Infrastructure.Persistence.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddProblemDetails();

builder.RegisterServices();

var app = builder.Build();

app.RegisterMiddlewares();

app.RegisterUserEndpoints();

app.EnsureCreatedDatabase();

app.Run();