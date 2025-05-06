using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Interfaces.IRepositories;

public interface ISettingRepository
{
	SettingEntity GetSetting(SettingKey key);
	T GetValue<T>(SettingKey key);
	Task<T?> GetValueAsync<T>(SettingKey key);
	void SetValue(SettingKey key, string value);
}
