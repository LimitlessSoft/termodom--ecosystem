using LSCore.DependencyInjection;
using LSCore.Exceptions.DependencyInjection;
using LSCore.Logging;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from json file and environment variables
builder
	.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddEnvironmentVariables();
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
builder.Services.AddControllers();
builder.AddLSCoreDependencyInjection("TD.OfficeServer");
builder.AddLSCoreLogging();
var app = builder.Build();
app.UseLSCoreExceptionsHandler();
app.MapControllers();
app.Run();
