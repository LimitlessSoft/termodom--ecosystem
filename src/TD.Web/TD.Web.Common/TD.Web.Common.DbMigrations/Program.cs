using TD.Common.Vault.DependencyInjection;
using TD.Web.Common.Repository;

var builder = WebApplication.CreateBuilder(args);
builder
	.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
	.AddEnvironmentVariables()
	.AddVault<SecretsDto>();
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
builder.Services.RegisterDatabase();
var app = builder.Build();
app.Run();

public class SecretsDto
{
	public string DEPLOY_ENV { get; set; }
	public string POSTGRES_HOST { get; set; }
	public string POSTGRES_PASSWORD { get; set; }
	public int POSTGRES_PORT { get; set; }
	public string POSTGRES_USER { get; set; }
}
