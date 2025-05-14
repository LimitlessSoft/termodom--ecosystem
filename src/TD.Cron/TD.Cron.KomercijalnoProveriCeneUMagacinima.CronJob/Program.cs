using LSCore.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TD.Common.Vault.DependencyInjection;
using TD.Office.Common.Contracts.Dtos;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.Common.Repository;
using TD.Office.Common.Repository.Repositories;
using TD.Office.KomercijalnoProveriCeneUMagacinima.Contracts.Interfaces.IManagers;
using TD.Office.KomercijalnoProveriCeneUMagacinima.Domain.Managers;

var builder = WebApplication.CreateBuilder(args);
builder
	.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
	.AddEnvironmentVariables()
	.AddVault<SecretsDto>();

builder.AddLSCoreLogging();
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
builder.Services.RegisterDatabase();
builder.Services.AddSingleton<ISettingRepository, SettingRepository>();
builder.Services.AddSingleton<
	IKomercijalnoProveriCeneUMagacinimaManager,
	KomercijalnoProveriCeneUMagacinimaManager
>();
var app = builder.Build();
var settingRepository = app.Services.GetService<IKomercijalnoProveriCeneUMagacinimaManager>()!;
await settingRepository.GenerateReportAsync();
