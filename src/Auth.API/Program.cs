using Auth.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiDi(builder.Configuration);

var app = builder.Build();

app.Run();