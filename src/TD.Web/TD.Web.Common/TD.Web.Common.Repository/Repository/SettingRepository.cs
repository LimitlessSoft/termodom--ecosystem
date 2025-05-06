using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Common.Repository.Repository;

public class SettingRepository(WebDbContext dbContext, IWebDbContextFactory dbContextFactory)
	: ISettingRepository
{
	public SettingEntity GetSetting(SettingKey key) =>
		dbContext.Settings.First(x => x.IsActive && x.Key == key);

	public T GetValue<T>(SettingKey key)
	{
		var setting = GetSetting(key);
		return (T)Convert.ChangeType(setting.Value, typeof(T));
	}

	public Task<T?> GetValueAsync<T>(SettingKey key)
	{
		return dbContextFactory
			.Create<WebDbContext>()
			.Settings.Where(x => x.IsActive && x.Key == key)
			.Select(x => (T)Convert.ChangeType(x.Value, typeof(T)))
			.FirstOrDefaultAsync();
	}

	public void SetValue(SettingKey key, string value)
	{
		var setting = GetSetting(key);
		setting.Value = value;
		dbContext.SaveChanges();
	}
}
