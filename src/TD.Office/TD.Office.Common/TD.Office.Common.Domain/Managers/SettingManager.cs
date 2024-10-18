using LSCore.Contracts;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IManagers;
using TD.Office.Common.Repository;

namespace TD.Office.Common.Domain.Managers;

public class SettingManager(
    ILogger<SettingManager> logger,
    OfficeDbContext dbContext,
    LSCoreContextUser currentUser
)
    : LSCoreManagerBase<SettingManager, SettingEntity>(logger, dbContext, currentUser),
        ISettingManager 
{ 
    public string GetValueByKey(SettingKey key) => 
        Queryable<SettingEntity>()
        .First(x =>
            x.IsActive &&
            x.Key == key.ToString()
        ).Value;
}
