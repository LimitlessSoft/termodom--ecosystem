using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IRepositories;

namespace TD.Office.Common.Repository.Repositories;

public class SettingRepository(OfficeDbContext dbContext) : ISettingRepository
{
	public SettingEntity? GetOrDefault(SettingKey key) =>
		dbContext.Settings.FirstOrDefault(x => x.IsActive && x.Key == key.ToString());

	public SettingEntity Get(SettingKey key) => GetOrDefault(key)!;

	public T GetValue<T>(SettingKey key) => (T)Convert.ChangeType(Get(key).Value, typeof(T));

	public T? GetValueOrDefault<T>(SettingKey key)
	{
		var setting = GetOrDefault(key);
		if (setting == null)
			return default;
		return (T)Convert.ChangeType(setting.Value, typeof(T));
	}

	public IEnumerable<SettingEntity> ByKey(SettingKey key) =>
		dbContext.Settings.Where(x => x.IsActive && x.Key == key.ToString());

	public void SetValue<T>(SettingKey key, T value)
	{
		var exists = false;
		var setting = GetOrDefault(key);
		if (setting != null)
		{
			exists = true;
		}
		else
		{
			setting = new SettingEntity
			{
				Key = key.ToString(),
				IsActive = true,
				CreatedAt = DateTime.UtcNow,
				CreatedBy = 0
			};
		}
		setting.Value = value!.ToString()!;
		if (!exists)
			dbContext.Settings.Add(setting);
		dbContext.SaveChanges();
	}
}
