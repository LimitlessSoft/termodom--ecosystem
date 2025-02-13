using TD.Web.Common.Contracts.Configurations;
using LSCore.DependencyInjection.Extensions;
using TD.Common.Vault.DependencyInjection;
using TD.Web.Common.Repository.Repository;
using TD.Web.Common.Contracts.Helpers;
using LSCore.Contracts.Configurations;
using TD.Web.Common.Domain.Managers;
using TD.Web.Admin.Contracts.Vault;
using LSCore.Framework.Extensions;
using TD.Web.Common.Repository;

var builder = WebApplication.CreateBuilder(args);

AddCommon(builder);
AddCors(builder);
builder.AddLSCoreDependencyInjection("TD.Web");
// builder.AddLSCoreApiKeyAuthorization(new LSCoreApiKeyConfiguration
// {
//     ApiKeys = ["2v738br3t89abtv8079yfc9q324yr7n7qw089rcft3y2w978"]
// });
AddAuthorization(builder);
AddMinio(builder);
builder.Services.RegisterDatabase();
builder.LSCoreAddLogging();
var app = builder.Build();
app.UseLSCoreHandleException();
app.UseCors("default");
app.UseLSCoreDependencyInjection();
// app.UseLSCoreApiKeyAuthorization();
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
            Audience = "office-termodom",
            Issuer = "office-termodom",
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