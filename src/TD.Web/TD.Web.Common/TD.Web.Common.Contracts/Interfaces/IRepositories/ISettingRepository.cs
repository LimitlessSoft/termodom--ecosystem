using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Interfaces.IRepositories;

public interface ISettingRepository
{
	SettingEntity GetSetting(SettingKey key);
	SettingEntity? GetSettingOrDefault(SettingKey key);
	List<SettingEntity> GetSettingsByPrefix(string prefix);
	T GetValue<T>(SettingKey key);
	Task<T> GetValueAsync<T>(SettingKey key);
	void SetValue(SettingKey key, string value);
	void SetOrCreateValue(SettingKey key, string value);
}
