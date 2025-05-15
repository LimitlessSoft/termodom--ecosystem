using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.IRepositories;

public interface ISettingRepository
{
	SettingEntity? GetOrDefault(SettingKey key);
	SettingEntity Get(SettingKey key);
	T GetValue<T>(SettingKey key);
	T? GetValueOrDefault<T>(SettingKey key);
	IEnumerable<SettingEntity> ByKey(SettingKey key);
	void SetValue<T>(SettingKey key, T value);
}
