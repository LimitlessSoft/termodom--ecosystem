using LSCore.ApiClient.Rest;
using LSCore.ApiClient.Rest.DependencyInjection;
using LSCore.Auth.UserPass.Contracts;
using LSCore.Auth.UserPass.DependencyInjection;
using LSCore.DependencyInjection;
using LSCore.Exceptions.DependencyInjection;
using LSCore.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Omu.ValueInjecter;
using StackExchange.Redis;
using TD.Common.Vault.DependencyInjection;
using TD.Office.Common.Repository;
using TD.Office.InterneOtpremnice.Client;
using TD.Office.MassSMS.Client;
using TD.Office.Public.Contracts.Dtos.Vault;
using TD.Office.Public.Domain.Managers;
using TD.Office.Public.Repository.Repositories;

var builder = WebApplication.CreateBuilder(args);

AddCommon(builder);
AddRedis(builder);
AddCors(builder);
AddAuthorization(builder);
AddInterneOtpremniceMicroserviceClient(builder);
AddMassSMSApiClient(builder);
builder.Services.RegisterDatabase();
builder.AddLSCoreDependencyInjection("TD.Office");
builder.AddLSCoreLogging();

var app = builder.Build();
app.UseLSCoreExceptionsHandler();
app.UseCors("default");
app.UseLSCoreDependencyInjection();
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
		InstanceName = "office-" + builder.Configuration["DEPLOY_ENV"] + "-",
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
