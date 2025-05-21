using TD.Common.Vault.DependencyInjection;
using TD.Office.Common.Contracts.Dtos;
using TD.Office.Common.Repository;

var builder = WebApplication.CreateBuilder(args);
builder
	.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
	.AddEnvironmentVariables()
	.AddVault<SecretsDto>();
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
builder.Services.RegisterDatabase();
var app = builder.Build();
app.Run();
