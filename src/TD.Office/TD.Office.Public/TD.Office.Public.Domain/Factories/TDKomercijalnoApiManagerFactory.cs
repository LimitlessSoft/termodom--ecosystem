using LSCore.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Contracts.IManagers;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.Public.Contracts.Interfaces.Factories;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Domain.Managers;

namespace TD.Office.Public.Domain.Factories;

public class TDKomercijalnoApiManagerFactory(
    ILogger<TDKomercijalnoApiManager> logger,
    LSCoreContextUser contextUser,
    IUserManager userManager,
    ISettingRepository settingRepository,
    ILogManager logManager,
    IConfigurationRoot configurationRoot
) : ITDKomercijalnoApiManagerFactory
{
    public ITDKomercijalnoApiManager Create(int year)
    {
        var komercijalnoApiManager = new TDKomercijalnoApiManager(
            logger,
            contextUser,
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
