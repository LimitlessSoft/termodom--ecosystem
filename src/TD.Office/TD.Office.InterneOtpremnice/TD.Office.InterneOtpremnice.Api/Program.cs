using LSCore.ApiClient.Rest;
using LSCore.ApiClient.Rest.DependencyInjection;
using LSCore.Contracts.Configurations;
using LSCore.DependencyInjection.Extensions;
using LSCore.Framework.Extensions;
using TD.Komercijalno.Client;
using TD.Office.InterneOtpremnice.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
builder.AddLSCoreApiKeyAuthorization(GenerateLSCoreApiKeyConfiguration());
builder.AddLSCoreDependencyInjection("TD.Office.InterneOtpremnice");
builder.AddLSCoreApiClientRest(LoadTDKomerijalnoClientConfiguration());
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<InterneOtpremniceDbContext>();
builder.Services.AddControllers();

var app = builder.Build();
app.UseLSCoreHandleException();
app.UseLSCoreApiKeyAuthorization();
app.MapControllers();
app.Run();

return;

LSCoreApiKeyConfiguration GenerateLSCoreApiKeyConfiguration()
{
#if DEBUG
    return new LSCoreApiKeyConfiguration() { ApiKeys = ["develop"] };
#else
    var apiKeysArray = builder.Configuration.GetSection("ApiKeys");
    var apiKeys = new HashSet<string>();
    apiKeysArray.Bind(apiKeys);
    return new LSCoreApiKeyConfiguration { ApiKeys = apiKeys };
#endif
}

LSCoreApiClientRestConfiguration<TDKomercijalnoClient> LoadTDKomerijalnoClientConfiguration()
{
#if DEBUG
    var environment = TDKomercijalnoEnvironment.Development;
#else
    var environment = TDKomercijalnoEnvironment.Production;
#endif
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
