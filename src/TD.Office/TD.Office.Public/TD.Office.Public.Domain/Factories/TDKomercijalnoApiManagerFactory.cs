using LSCore.Contracts;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Contracts.IManagers;
using TD.Office.Public.Contracts.Interfaces.Factories;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Domain.Managers;

namespace TD.Office.Public.Domain.Factories;

public class TDKomercijalnoApiManagerFactory(
    ILogger<TDKomercijalnoApiManager> logger,
    LSCoreContextUser contextUser,
    IUserManager userManager,
    ISettingManager settingManager,
    ILogManager logManager)
    : ITDKomercijalnoApiManagerFactory
{
    public ITDKomercijalnoApiManager Create(int year)
    {
        var komercijalnoApiManager = new TDKomercijalnoApiManager(logger, contextUser, userManager, settingManager, logManager);
        komercijalnoApiManager.SetYear(year);
        return komercijalnoApiManager;
    }
}