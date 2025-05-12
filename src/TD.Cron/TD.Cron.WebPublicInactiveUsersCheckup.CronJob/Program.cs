using LSCore.Logging;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Common.Repository;
using TD.Web.Common.Repository.Repository;

var builder = WebApplication.CreateBuilder(args);
builder
	.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
	.AddEnvironmentVariables();

builder.AddLSCoreLogging();
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
builder.Services.RegisterDatabase();
builder.Services.AddSingleton<ISettingRepository, SettingRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

var app = builder.Build();

var userRepository = app.Services.GetService<IUserRepository>()!;
var settingRepository = app.Services.GetService<ISettingRepository>()!;

var inactivityPeriod = settingRepository.GetValue<int>(
	SettingKey.AZURIRAJ_NIVO_NEAKTIVNIM_KORISNICIMA_NEAKTIVAN_JE_AKO_NIJE_KUPOVAO_DANA
);
var finalLevelForInactiveUsers = settingRepository.GetValue<int>(
	SettingKey.AZURIRAJ_NIVO_NEAKTIVNIM_KORISNICIMA_DESTINACIONI_NIVO
);
var isThisCronJobEnabled = settingRepository.GetValue<bool>(
	SettingKey.FEATURE_CRON_AZURIRAJ_NIVO_NEAKTIVNIM_KORISNICIMA_ACTIVE
);

if (isThisCronJobEnabled == false)
	return;

var inactiveUsers = userRepository.GetInactiveUsers(TimeSpan.FromDays(inactivityPeriod));

foreach (var priceLevel in inactiveUsers.SelectMany(user => user.ProductPriceGroupLevels))
	priceLevel.Level = finalLevelForInactiveUsers;

userRepository.Update(inactiveUsers);
