using LSCore.DependencyInjection;
using TD.Komercijalno.Repository;

var builder = WebApplication.CreateBuilder(args);

builder
	.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
	.AddEnvironmentVariables();
builder.AddLSCoreDependencyInjection("TD.Komercijalno");
builder.Services.RegisterDatabase(builder.Configuration);

var app = builder.Build();
app.MapControllers();
app.Run();
