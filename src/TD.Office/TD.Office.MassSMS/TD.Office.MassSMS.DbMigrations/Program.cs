using TD.Common.Vault.DependencyInjection;
using TD.Office.MassSMS.Contracts.Dtos;
using TD.Office.MassSMS.Repository;

var builder = WebApplication.CreateBuilder(args);
builder
	.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
	.AddEnvironmentVariables()
	.AddVault<SecretsDto>();
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<MassSMSContext>();
var app = builder.Build();
app.Run();
