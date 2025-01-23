using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TD.Office.InterneOtpremnice.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<InterneOtpremniceDbContext>();
var app = builder.Build();
app.Run();
