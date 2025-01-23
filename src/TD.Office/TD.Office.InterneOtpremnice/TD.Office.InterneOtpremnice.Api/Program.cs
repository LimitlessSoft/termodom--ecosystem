using LSCore.Contracts.Configurations;
using LSCore.DependencyInjection.Extensions;
using LSCore.Framework.Extensions;
using TD.Office.InterneOtpremnice.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
builder.AddLSCoreApiKeyAuthorization(GenerateLSCoreApiKeyConfiguration());
builder.AddLSCoreDependencyInjection("TD.Office.InterneOtpremnice");
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
