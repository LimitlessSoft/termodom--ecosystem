using Hangfire;
using Hangfire.PostgreSql;
using LSCore.ApiClient.Rest;
using LSCore.ApiClient.Rest.DependencyInjection;
using LSCore.Auth.Key.Contracts;
using LSCore.Auth.Key.DependencyInjection;
using LSCore.DependencyInjection;
using LSCore.Exceptions.DependencyInjection;
using TD.Common.Vault.DependencyInjection;
using TD.Office.MassSMS.Contracts.Constants;
using TD.Office.MassSMS.Contracts.Dtos;
using TD.Office.MassSMS.Contracts.Interfaces;
using TD.Office.MassSMS.Repository;
using TD.OfficeServer.Client;

var builder = WebApplication.CreateBuilder(args);
builder
	.Configuration.AddJsonFile("appsettings.json", optional: true)
	.AddEnvironmentVariables()
	.AddVault<SecretsDto>();
builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
builder.AddLSCoreAuthKey(
	new LSCoreAuthKeyConfiguration
	{
		AuthAll = true,
		ValidKeys = [.. builder.Configuration.GetSection("API_KEYS").Value!.Split(",")]
	}
);
builder.AddLSCoreDependencyInjection("TD.Office.MassSMS");
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<MassSMSContext>();
builder.Services.AddSingleton<IMassSMSDbContextFactory, MassSMSDbContextFactory>();
builder.Services.AddHangfire(config =>
{
	config.UsePostgreSqlStorage(options =>
	{
		options.UseNpgsqlConnection(DbConstants.ConnectionString(builder.Configuration));
	});
});
builder.Services.AddHangfireServer();
builder.AddLSCoreApiClientRest(
	new LSCoreApiClientRestConfiguration<TDOfficeServerClient>
	{
		BaseUrl = builder.Configuration["OFFICE_SERVER_BASE_URL"]!
	}
);
var app = builder.Build();
app.UseLSCoreDependencyInjection();
app.UseLSCoreExceptionsHandler();
#if !DEBUG
app.UseLSCoreAuthKey();
#endif
app.MapControllers();
app.Run();
