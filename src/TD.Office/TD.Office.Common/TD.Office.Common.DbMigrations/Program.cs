using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TD.Office.Common.Repository;

var builder = WebApplication.CreateBuilder(args);
builder
    .Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
builder.Services.RegisterDatabase(builder.Configuration);
var app = builder.Build();
app.Run();
