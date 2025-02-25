using LSCore.Contracts.Configurations;
using LSCore.DependencyInjection.Extensions;
using LSCore.Framework.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Omu.ValueInjecter;
using StackExchange.Redis;
using TD.Common.Vault.DependencyInjection;
using TD.Web.Common.Contracts.Configurations;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Domain.Managers;
using TD.Web.Common.Repository;
using TD.Web.Common.Repository.Repository;
using TD.Web.Public.Contracts.Dtos.Vault;

var builder = WebApplication.CreateBuilder(args);
AddCommon(builder);
AddRedis(builder);
AddCors(builder);
builder.AddLSCoreDependencyInjection("TD.Web");
AddAuthorization(builder);
AddMinio(builder);
builder.Services.RegisterDatabase();
builder.Services.AddSingleton<IWebDbContextFactory, WebDbContextFactory>();
builder.LSCoreAddLogging();

var app = builder.Build();
app.UseLSCoreHandleException();
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
            Audience = "web-public-termodom",
            Issuer = "web-public-termodom",
            SecurityKey = builder.Configuration["JWT_KEY"]!,
        }
    );
}
static void AddMinio(WebApplicationBuilder builder)
{
    builder.Services.AddSingleton(
        new MinioConfiguration
        {
            BucketBase = GeneralHelpers.GenerateBucketName(builder.Configuration["DEPLOY_ENV"]!),
            Host = builder.Configuration["MINIO_HOST"]!,
            AccessKey = builder.Configuration["MINIO_ACCESS_KEY"]!,
            SecretKey = builder.Configuration["MINIO_SECRET_KEY"]!,
            Port = builder.Configuration["MINIO_PORT"]!
        });
}
static void AddRedis(WebApplicationBuilder builder)
{
    var redisCacheOptions = new RedisCacheOptions()
    {
        InstanceName = "web-" + builder.Configuration["DEPLOY_ENV"] + "-",
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
    builder.Services.AddScoped<IDistributedCache, RedisCache>(x => new RedisCache(redisCacheOptions));
}