using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Contracts.IManagers
{
    public interface ISettingManager
    {
        IQueryable<SettingEntity> Queryable();
        string GetByKey(string key);
    }
}
