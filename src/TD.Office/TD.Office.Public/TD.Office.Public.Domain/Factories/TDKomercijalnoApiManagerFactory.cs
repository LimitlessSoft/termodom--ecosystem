using Microsoft.Extensions.Configuration;
using TD.Office.Common.Contracts.IManagers;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.Public.Contracts.Interfaces.Factories;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Domain.Managers;

namespace TD.Office.Public.Domain.Factories;

public class TDKomercijalnoApiManagerFactory(
	IUserManager userManager,
	ISettingRepository settingRepository,
	ILogManager logManager,
	IConfigurationRoot configurationRoot
) : ITDKomercijalnoApiManagerFactory
{
	public ITDKomercijalnoApiManager Create(int year)
	{
		var komercijalnoApiManager = new TDKomercijalnoApiManager(
			userManager,
			settingRepository,
			logManager,
			configurationRoot
		);
		komercijalnoApiManager.SetYear(year);
		return komercijalnoApiManager;
	}

	public Dictionary<int, ITDKomercijalnoApiManager> Create(List<int> year) =>
		year.ToDictionary(y => y, Create);
}
