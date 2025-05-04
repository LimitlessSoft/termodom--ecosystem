using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IRepositories;

namespace TD.Office.Common.Repository.Repositories;

public class SettingRepository(OfficeDbContext dbContext) : ISettingRepository
{
	public SettingEntity Get(SettingKey key) =>
		dbContext.Settings.First(x => x.IsActive && x.Key == key.ToString());

	public T GetValue<T>(SettingKey key) => (T)Convert.ChangeType(Get(key).Value, typeof(T));

	public IEnumerable<SettingEntity> ByKey(SettingKey key) =>
		dbContext.Settings.Where(x => x.IsActive && x.Key == key.ToString());
}
