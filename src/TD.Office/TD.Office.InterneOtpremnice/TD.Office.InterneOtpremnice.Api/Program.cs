using LSCore.ApiClient.Rest;
using LSCore.DependencyInjection;
using TD.Common.Environments;
using TD.Common.Vault.DependencyInjection;
using TD.Komercijalno.Client;
using TD.Office.InterneOtpremnice.Contracts.Dtos.Vault;
using TD.Office.InterneOtpremnice.Repository;
using Environment = TD.Common.Environments.Environment;

var builder = WebApplication.CreateBuilder(args);
builder
	.Configuration.AddJsonFile("appsettings.json")
	.AddEnvironmentVariables()
	.AddVault<SecretsDto>();
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);

// builder.AddLSCoreApiKeyAuthorization(GenerateLSCoreApiKeyConfiguration());
builder.AddLSCoreDependencyInjection("TD.Office.InterneOtpremnice");

// builder.AddLSCoreApiClientRest(LoadTDKomerijalnoClientConfiguration());
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<InterneOtpremniceDbContext>();
builder.Services.AddControllers();

var app = builder.Build();
app.UseLSCoreDependencyInjection();

// app.UseLSCoreHandleException();
// app.UseLSCoreApiKeyAuthorization();
app.MapControllers();
app.Run();

return;

// LSCoreApiKeyConfiguration GenerateLSCoreApiKeyConfiguration() =>
// 	new()
// 	{
// 		ApiKeys = [.. builder.Configuration.GetSection("API_KEYS").Value!.Split(",")],
// 		AuthorizeAll = true
// 	};

LSCoreApiClientRestConfiguration<TDKomercijalnoClient> LoadTDKomerijalnoClientConfiguration()
{
	var environment = builder.Configuration[
		TD.Common.Environments.Constants.DeployVariable
	]!.ResolveDeployVariable() switch
	{
		Environment.Development => TDKomercijalnoEnvironment.Development,
		Environment.Production => TDKomercijalnoEnvironment.Production,
		Environment.Stage => throw new NotImplementedException(), // Not sure what here should be
		Environment.Automation => throw new NotImplementedException(), // Not sure what here should be
		_ => throw new ArgumentException("Invalid environment")
	};
	var configuration = new LSCoreApiClientRestConfiguration<TDKomercijalnoClient>
	{
		BaseUrl = TD.Komercijalno.Client.Constants.KomercijalnoApiUrlFormat(
			DateTime.UtcNow.Year,
			environment,
			TDKomercijalnoFirma.TCMDZ
		)
	};
	return configuration;
}
