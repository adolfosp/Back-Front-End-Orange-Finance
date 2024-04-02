using OrangeFinance;
using OrangeFinance.Application;
using OrangeFinance.Endpoints;
using OrangeFinance.Extensions;
using OrangeFinance.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation();
builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.RegisterServices();

var app = builder.Build();

app.RegisterMiddlewares();

app.RegisterUserEndpoints();

app.Run();