using LSCore.ApiClient.Rest;
using LSCore.ApiClient.Rest.DependencyInjection;
using LSCore.Contracts.Configurations;
using LSCore.DependencyInjection.Extensions;
using LSCore.Framework.Extensions;
using TD.Common.Vault.DependencyInjection;
using TD.Komercijalno.Client;
using TD.Office.InterneOtpremnice.Contracts.Dtos.Vault;
using TD.Office.InterneOtpremnice.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddVault<SecretsDto>();
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
builder.AddLSCoreApiKeyAuthorization(GenerateLSCoreApiKeyConfiguration());
builder.AddLSCoreDependencyInjection("TD.Office.InterneOtpremnice");
builder.AddLSCoreApiClientRest(LoadTDKomerijalnoClientConfiguration());
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<InterneOtpremniceDbContext>();
builder.Services.AddControllers();

var app = builder.Build();
app.UseLSCoreDependencyInjection();
app.UseLSCoreHandleException();
app.UseLSCoreApiKeyAuthorization();
app.MapControllers();
app.Run();

return;

LSCoreApiKeyConfiguration GenerateLSCoreApiKeyConfiguration() => new ()
    {
        ApiKeys = [..builder.Configuration.GetSection("API_KEYS").Value!.Split(",")]
    };

LSCoreApiClientRestConfiguration<TDKomercijalnoClient> LoadTDKomerijalnoClientConfiguration()
{
    var environment = builder.Configuration["DEPLOY_ENV"] switch
    {
        "develop" => TDKomercijalnoEnvironment.Development,
        "production" => TDKomercijalnoEnvironment.Production,
        _ => throw new ArgumentException("Invalid environment")
    };
    var configuration = new LSCoreApiClientRestConfiguration<TDKomercijalnoClient>
    {
        BaseUrl = Constants.KomercijalnoApiUrlFormat(
            DateTime.UtcNow.Year,
            environment,
            TDKomercijalnoFirma.TCMDZ
        )
    };
    return configuration;
}
