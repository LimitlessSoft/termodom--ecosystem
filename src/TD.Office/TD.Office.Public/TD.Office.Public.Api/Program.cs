using LSCore.ApiClient.Rest;
using LSCore.ApiClient.Rest.DependencyInjection;
using LSCore.Auth.Key.Contracts;
using LSCore.Auth.Key.DependencyInjection;
using LSCore.Auth.UserPass.Contracts;
using LSCore.Auth.UserPass.DependencyInjection;
using LSCore.DependencyInjection;
using LSCore.Exceptions.DependencyInjection;
using LSCore.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Omu.ValueInjecter;
using StackExchange.Redis;
using TD.Common.Environments;
using TD.Common.Vault.DependencyInjection;
using TD.Komercijalno.Client;
using TD.Office.Common.Contracts.Dtos;
using TD.Office.Common.Repository;
using TD.Office.InterneOtpremnice.Client;
using TD.Office.KomercijalnoProveriCeneUMagacinima.Contracts.Interfaces.IManagers;
using TD.Office.KomercijalnoProveriCeneUMagacinima.Domain.Managers;
using TD.Office.MassSMS.Client;
using TD.Office.Public.Domain.Managers;
using TD.Office.Public.Repository.Repositories;
using Constants = TD.Common.Environments.Constants;
using Environment = TD.Common.Environments.Environment;

var builder = WebApplication.CreateBuilder(args);

AddCommon(builder);
AddRedis(builder);
AddCors(builder);
AddAuthorization(builder);
AddInterneOtpremniceMicroserviceClient(builder);
AddMassSMSApiClient(builder);
builder.Services.AddSingleton<ITDKomercijalnoClientFactory, TDKomercijalnoClientFactory>();
builder.AddLSCoreApiClientRest(LoadTDKomerijalnoDefaultClientConfiguration());
builder.Services.RegisterDatabase();
builder.AddLSCoreDependencyInjection("TD.Office");
builder.Services.AddScoped<
	IKomercijalnoProveriCeneUMagacinimaManager,
	KomercijalnoProveriCeneUMagacinimaManager
>();
builder.AddLSCoreLogging();

var app = builder.Build();
app.UseLSCoreExceptionsHandler();
app.UseCors("default");
app.UseLSCoreDependencyInjection();
app.UseLSCoreAuthKey();
app.UseLSCoreAuthUserPass<string>();
app.MapControllers();
app.Run();

return;

static void AddMassSMSApiClient(WebApplicationBuilder builder)
{
	builder.AddLSCoreApiClientRest(
		new LSCoreApiClientRestConfiguration<MassSMSApiClient>
		{
			BaseUrl = builder.Configuration["OFFICE_MASS_SMS_BASE_URL"]!,
			LSCoreApiKey = builder.Configuration["OFFICE_MASS_SMS_API_KEY"]
		}
	);
}
static void AddCommon(WebApplicationBuilder builder)
{
	builder
		.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
		.AddEnvironmentVariables()
		.AddVault<SecretsDto>();

	builder.Services.AddControllers();
	builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
	builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
}
static void AddRedis(WebApplicationBuilder builder)
{
	var redisCacheOptions = new RedisCacheOptions()
	{
		InstanceName = "office-" + builder.Configuration[Constants.DeployVariable] + "-",
		ConfigurationOptions = new ConfigurationOptions()
		{
			EndPoints = new EndPointCollection() { { "85.90.245.17", 6379 }, },
			SyncTimeout = 30 * 1000
		}
	};
	builder.Services.AddStackExchangeRedisCache(x =>
	{
		x.InjectFrom(redisCacheOptions);
	});
	builder.Services.AddScoped<IDistributedCache, RedisCache>(x => new RedisCache(
		redisCacheOptions
	));
}
static void AddCors(WebApplicationBuilder builder)
{
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
}
static void AddAuthorization(WebApplicationBuilder builder)
{
	builder.AddLSCoreAuthKey<ApiKeysProvider>(
		new LSCoreAuthKeyConfiguration { AuthAll = true, BreakOnFailedAuth = false }
	);
	builder.AddLSCoreAuthUserPass<string, AuthManager, UserRepository>(
		new LSCoreAuthUserPassConfiguration()
		{
			AccessTokenExpirationMinutes = 60 * 12,
			Audience = "office-public-termodom",
			Issuer = "office-public-termodom",
			SecurityKey = builder.Configuration["JWT_KEY"]!,
		}
	);
}
static void AddInterneOtpremniceMicroserviceClient(WebApplicationBuilder builder)
{
	builder.AddLSCoreApiClientRest(
		new LSCoreApiClientRestConfiguration<TDOfficeInterneOtpremniceClient>()
		{
#if DEBUG
			BaseUrl = $"http://localhost:5262",
#else
			BaseUrl =
				$"http://{builder.Configuration[TD.Common.Environments.Constants.DeployVariable]}-office-interne-otpremnice-api-service:81",
#endif
			LSCoreApiKey = builder.Configuration["TD_INTERNE_OTPREMNICE_MICROSERVICE_API_KEY"]!
		}
	);
}
LSCoreApiClientRestConfiguration<TDKomercijalnoClient> LoadTDKomerijalnoDefaultClientConfiguration()
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
