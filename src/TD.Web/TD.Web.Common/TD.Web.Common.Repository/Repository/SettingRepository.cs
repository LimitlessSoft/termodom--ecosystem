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

	public SettingEntity? GetSettingOrDefault(SettingKey key) =>
		dbContext.Settings.FirstOrDefault(x => x.IsActive && x.Key == key);

	public List<SettingEntity> GetSettingsByPrefix(string prefix) =>
		dbContext.Settings
			.Where(x => x.IsActive && x.Key.ToString().StartsWith(prefix))
			.ToList();

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

	public void SetOrCreateValue(SettingKey key, string value)
	{
		var setting = GetSettingOrDefault(key);
		if (setting == null)
		{
			setting = new SettingEntity { Key = key, Value = value };
			dbContext.Settings.Add(setting);
		}
		else
		{
			setting.Value = value;
		}
		dbContext.SaveChanges();
	}
}
