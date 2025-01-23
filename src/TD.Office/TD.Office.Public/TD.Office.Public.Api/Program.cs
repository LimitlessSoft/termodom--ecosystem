using LSCore.Contracts.Configurations;
using LSCore.DependencyInjection.Extensions;
using LSCore.Framework.Extensions;
using LSCore.Framework.Middlewares;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Omu.ValueInjecter;
using StackExchange.Redis;
using TD.Office.Common.Repository;
using TD.Office.Public.Domain.Managers;
using TD.Office.Public.Repository.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder
    .Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

#region Redis
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
builder.Services.AddScoped<IDistributedCache, RedisCache>(x => new RedisCache(redisCacheOptions));
#endregion

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

builder.AddLSCoreAuthorization<AuthManager, UserRepository>(
    new LSCoreAuthorizationConfiguration
    {
        Audience = "office-termodom",
        Issuer = "office-termodom",
        SecurityKey = builder.Configuration["JWT_KEY"]!,
    }
);

builder.Services.RegisterDatabase(builder.Configuration);

builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
builder.AddLSCoreDependencyInjection("TD.Office");
builder.LSCoreAddLogging();
builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<LSCoreHandleExceptionMiddleware>();
app.UseCors("default");
app.UseLSCoreDependencyInjection();
app.UseLSCoreAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
