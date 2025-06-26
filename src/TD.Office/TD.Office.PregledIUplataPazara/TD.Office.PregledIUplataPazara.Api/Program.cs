using LSCore.ApiClient.Rest;
using LSCore.ApiClient.Rest.DependencyInjection;
using LSCore.Auth.Key.Contracts;
using LSCore.Auth.Key.DependencyInjection;
using LSCore.DependencyInjection;
using LSCore.Exceptions.DependencyInjection;
using TD.Common.Vault.DependencyInjection;
using TD.Komercijalno.Client;
using TD.Office.PregledIUplataPazara.Contracts.Dtos;
using TD.Office.PregledIUplataPazara.Domain.Managers;
using TD.Office.Public.Client;

var builder = WebApplication.CreateBuilder(args);
builder
    .Configuration.AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .AddVault<SecretsDto>();
builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
builder.AddLSCoreAuthKey<AuthKeyProvider>(
    new LSCoreAuthKeyConfiguration
    {
        AuthAll = true,
    }
);
builder.AddLSCoreDependencyInjection("TD.Office.PregledIUplataPazara");
builder.Services.AddSingleton<ITDKomercijalnoClientFactory, TDKomercijalnoClientFactory>();
builder.AddLSCoreApiClientRest(LoadTDOfficeClientConfiguration());
var app = builder.Build();
app.UseLSCoreDependencyInjection();
app.UseLSCoreExceptionsHandler();
#if !DEBUG
app.UseLSCoreAuthKey();
#endif
app.MapControllers();
app.Run();

return;

LSCoreApiClientRestConfiguration<TDOfficeClient> LoadTDOfficeClientConfiguration()
{
    return new LSCoreApiClientRestConfiguration<TDOfficeClient>
    {
        BaseUrl = builder.Configuration["OFFICE_API_BASE_URL"]!,
        LSCoreApiKey = builder.Configuration["OFFICE_API_KEY"]
    };
}