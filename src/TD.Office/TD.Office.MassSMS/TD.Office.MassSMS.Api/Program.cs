using Hangfire;
using Hangfire.PostgreSql;
using LSCore.Auth.Key.Contracts;
using LSCore.Auth.Key.DependencyInjection;
using LSCore.DependencyInjection;
using TD.Common.Vault.DependencyInjection;
using TD.Office.MassSMS.Contracts;
using TD.Office.MassSMS.Contracts.Constants;
using TD.Office.MassSMS.Contracts.Dtos;
using TD.Office.MassSMS.Repository;

var builder = WebApplication.CreateBuilder(args);
builder
	.Configuration.AddJsonFile("appsettings.json")
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
builder.Services.AddHangfire(config =>
{
	config.UsePostgreSqlStorage(options =>
	{
		options.UseNpgsqlConnection(DbConstants.ConnectionString(builder.Configuration));
	});
});
builder.Services.AddHangfireServer();
var app = builder.Build();
app.UseLSCoreDependencyInjection();
#if !DEBUG
app.UseLSCoreAuthKey();
#endif
app.MapControllers();
app.Run();
