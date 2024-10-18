using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.IManagers;

public interface ISettingManager
{
    IQueryable<SettingEntity> Queryable();
    string GetValueByKey(SettingKey key);
}
