using OrangeFinance;
using OrangeFinance.Endpoints;
using OrangeFinance.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation();

builder.RegisterServices();

var app = builder.Build();

app.RegisterMiddlewares();

app.RegisterUserEndpoints();

app.Run();