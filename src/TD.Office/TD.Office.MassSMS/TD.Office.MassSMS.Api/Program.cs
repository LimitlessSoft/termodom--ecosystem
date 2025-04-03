using LSCore.Auth.Key.Contracts;
using LSCore.Auth.Key.DependencyInjection;
using LSCore.DependencyInjection;
using TD.Common.Vault.DependencyInjection;
using TD.Office.MassSMS.Contracts;
using TD.Office.MassSMS.Contracts.Dtos;

var builder = WebApplication.CreateBuilder(args);
builder
	.Configuration.AddJsonFile("appsettings.json")
	.AddEnvironmentVariables()
	.AddVault<SecretsDto>();
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
builder.AddLSCoreAuthKey(
	new LSCoreAuthKeyConfiguration
	{
		AuthAll = true,
		ValidKeys = [.. builder.Configuration.GetSection("API_KEYS").Value!.Split(",")]
	}
);
builder.AddLSCoreDependencyInjection("TD.Office.MassSMS");
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<MassSMSContext>()
var app = builder.Build();
app.Run();
