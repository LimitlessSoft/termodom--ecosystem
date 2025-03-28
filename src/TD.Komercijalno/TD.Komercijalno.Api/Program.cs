using LSCore.DependencyInjection;
using TD.Komercijalno.Repository;

var builder = WebApplication.CreateBuilder(args);

builder
	.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
	.AddEnvironmentVariables();
builder.Services.AddControllers();
builder.AddLSCoreDependencyInjection("TD.Komercijalno");
builder.Services.RegisterDatabase(builder.Configuration);

var app = builder.Build();
app.UseLSCoreDependencyInjection();
app.MapControllers();
app.Run();
