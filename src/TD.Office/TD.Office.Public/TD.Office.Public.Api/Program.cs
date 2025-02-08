using LSCore.ApiClient.Rest;
using LSCore.ApiClient.Rest.DependencyInjection;
using LSCore.Contracts.Configurations;
using LSCore.Contracts.Extensions;
using LSCore.DependencyInjection.Extensions;
using LSCore.Framework.Extensions;
using LSCore.Framework.Middlewares;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using StackExchange.Redis;
using TD.Common.Vault;
using TD.Common.Vault.DependencyInjection;
using TD.Office.Common.Repository;
using TD.Office.InterneOtpremnice.Client;
using TD.Office.Public.Contracts.Dtos.Vault;
using TD.Office.Public.Domain.Managers;
using TD.Office.Public.Repository.Repositories;

var builder = WebApplication.CreateBuilder(args);

AddCommon(builder);
AddRedis(builder);
AddCors(builder);
AddAuthorization(builder);
AddInterneOtpremniceMicroserviceClient(builder);
builder.AddLSCoreDependencyInjection("TD.Office");
builder.LSCoreAddLogging();
builder.Services.RegisterDatabase(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<LSCoreHandleExceptionMiddleware>();
app.UseCors("default");
app.UseLSCoreDependencyInjection();
app.UseLSCoreAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();

return;

static void AddCommon(WebApplicationBuilder builder)
{
    builder
        .Configuration
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .AddVault<SecretsDto>();

    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers();
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
}

static void AddRedis(WebApplicationBuilder builder)
{
    var redisCacheOptions = new RedisCacheOptions()
    {
        InstanceName = "office-" + builder.Configuration["POSTGRES_DATABASE_NAME"] + "-",
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
    builder.AddLSCoreAuthorization<AuthManager, UserRepository>(
        new LSCoreAuthorizationConfiguration
        {
            Audience = "office-termodom",
            Issuer = "office-termodom",
            SecurityKey = builder.Configuration["JWT_KEY"]!,
        }
    );
}

static void AddInterneOtpremniceMicroserviceClient(WebApplicationBuilder builder)
{
    var env = builder.Configuration["DEPLOY_ENV"] switch
    {
        "production" => TDInterneOtpremniceEnvironment.Production,
        _ => TDInterneOtpremniceEnvironment.Development,
    };
    builder.AddLSCoreApiClientRest(
        new LSCoreApiClientRestConfiguration<TDOfficeInterneOtpremniceClient>()
        {
#if DEBUG
            BaseUrl = $"http://localhost:5262",
#else
            BaseUrl = $"https://api-interne-otpremnice{env.GetDescription()}.termodom.rs",
#endif
            LSCoreApiKey = builder.Configuration["TD_INTERNE_OTPREMNICE_MICROSERVICE_API_KEY"]!
        }
    );
}
