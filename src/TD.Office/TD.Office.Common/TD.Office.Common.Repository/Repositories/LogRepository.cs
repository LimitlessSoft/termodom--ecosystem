using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IRepositories;

namespace TD.Office.Common.Repository.Repositories;

public class LogRepository(OfficeDbContext dbContext) : ILogRepository
{
	public void Create(LogKey key) => Create(key, string.Empty);

	public void Create(LogKey key, string value)
	{
		dbContext.Logs.Add(
			new LogEntity()
			{
				IsActive = true,
				Key = key,
				Value = value
			}
		);
		dbContext.SaveChanges();
	}

	public IQueryable<LogEntity> GetMultiple() => dbContext.Logs.Where(x => x.IsActive);
}
