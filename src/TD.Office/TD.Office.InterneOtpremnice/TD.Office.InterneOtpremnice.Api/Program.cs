using LSCore.ApiClient.Rest;
using LSCore.ApiClient.Rest.DependencyInjection;
using LSCore.Auth.Key.Contracts;
using LSCore.Auth.Key.DependencyInjection;
using LSCore.DependencyInjection;
using LSCore.Exceptions.DependencyInjection;
using TD.Common.Environments;
using TD.Common.Vault.DependencyInjection;
using TD.Komercijalno.Client;
using TD.Office.InterneOtpremnice.Contracts.Dtos.Vault;
using TD.Office.InterneOtpremnice.Repository;
using TD.Office.Public.Client;
using Constants = TD.Common.Environments.Constants;
using Environment = TD.Common.Environments.Environment;

var builder = WebApplication.CreateBuilder(args);
InitializeCommon();
InitializeAuth();
builder.AddLSCoreDependencyInjection("TD.Office.InterneOtpremnice");

builder.Services.AddSingleton<ITDKomercijalnoClientFactory, TDKomercijalnoClientFactory>();
builder.AddLSCoreApiClientRest(LoadTDKomerijalnoClientConfiguration());
builder.AddLSCoreApiClientRest(LoadTDOfficeClientConfiguration());
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<InterneOtpremniceDbContext>();
builder.Services.AddControllers();

var app = builder.Build();
app.UseLSCoreExceptionsHandler();
app.UseCors("default");
app.UseLSCoreDependencyInjection();
#if !DEBUG
app.UseLSCoreAuthKey();
#endif
app.MapControllers();
app.Run();

return;

void InitializeCommon()
{
	builder
		.Configuration.AddJsonFile("appsettings.json", optional: true)
		.AddEnvironmentVariables()
		.AddVault<SecretsDto>();
	builder.Services.AddCors(options =>
	{
		options.AddPolicy(
			"default",
			policy =>
			{
				policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
			}
		);
	});
	builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
}

void InitializeAuth()
{
	builder.AddLSCoreAuthKey(
		new LSCoreAuthKeyConfiguration
		{
			AuthAll = true,
			ValidKeys = [.. builder.Configuration.GetSection("API_KEYS").Value!.Split(",")]
		}
	);
}

LSCoreApiClientRestConfiguration<TDKomercijalnoClient> LoadTDKomerijalnoClientConfiguration()
{
	var environment = builder.Configuration[
		Constants.DeployVariable
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
LSCoreApiClientRestConfiguration<TDOfficeClient> LoadTDOfficeClientConfiguration()
{
	var configuration = new LSCoreApiClientRestConfiguration<TDOfficeClient>
	{
		BaseUrl = builder.Configuration["OFFICE_API_BASE_URL"]!,
		LSCoreApiKey = builder.Configuration["OFFICE_API_KEY"]
	};
	return configuration;
}
