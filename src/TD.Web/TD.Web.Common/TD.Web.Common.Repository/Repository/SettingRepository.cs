using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Common.Repository.Repository;

public class SettingRepository(WebDbContext dbContext) : ISettingRepository
{
	public SettingEntity GetSetting(SettingKey key) =>
		dbContext.Settings.First(x => x.IsActive && x.Key == key);

	public T GetValue<T>(SettingKey key)
	{
		var setting = GetSetting(key);
		return (T)Convert.ChangeType(setting.Value, typeof(T));
	}
}
