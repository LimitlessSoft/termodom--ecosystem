using LSCore.Exceptions;
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

	public async Task<T> GetValueAsync<T>(SettingKey key)
	{
		var context = dbContextFactory.Create<WebDbContext>();
		var setting = await context
			.Settings.AsNoTracking()
			.FirstOrDefaultAsync(x => x.IsActive && x.Key == key);
		if (setting == null)
			throw new LSCoreNotFoundException();
		return (T)Convert.ChangeType(setting.Value, typeof(T));
	}

	public void SetValue(SettingKey key, string value)
	{
		var setting = GetSetting(key);
		setting.Value = value;
		dbContext.SaveChanges();
	}
}
