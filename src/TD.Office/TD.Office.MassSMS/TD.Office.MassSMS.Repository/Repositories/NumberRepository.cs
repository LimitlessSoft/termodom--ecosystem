using LSCore.Exceptions;
using LSCore.Repository;
using TD.Office.MassSMS.Contracts.Entities;
using TD.Office.MassSMS.Contracts.Interfaces;
using TD.Office.MassSMS.Contracts.Interfaces.Repositories;

namespace TD.Office.MassSMS.Repository.Repositories;

public class NumberRepository(MassSMSContext dbContext, IMassSMSDbContextFactory dbContextFactory)
	: LSCoreRepositoryBase<NumberEntity>(dbContext),
		INumberRepository
{
	public NumberEntity? GetOrDefault(string number) =>
		dbContext.Numbers.FirstOrDefault(x => x.IsActive && x.Number == number);

	public NumberEntity Get(string number)
	{
		var entity = GetOrDefault(number);
		if (entity == null)
			throw new LSCoreNotFoundException();
		return entity;
	}

	public void Insert(string number) =>
		Insert(new NumberEntity { Number = number, IsActive = true });

	/// <summary>
	/// Fire and forget insert
	/// </summary>
	/// <param name="number"></param>
	/// <returns></returns>
	public Task InsertAsync(params string[] number) =>
		Task.Run(() =>
		{
			if (number.Length == 0)
				return;
			using var context = dbContextFactory.Create();
			var __numbers = context
				.Numbers.Where(x => number.Contains(x.Number))
				.Select(x => x.Number)
				.ToHashSet();
			foreach (var n in number)
			{
				if (__numbers.Contains(n))
					continue;
				context.Numbers.Add(
					new NumberEntity
					{
						Number = n,
						IsActive = true,
						CreatedBy = 0,
						CreatedAt = DateTime.UtcNow
					}
				);
			}
			context.SaveChanges();
		});

	public List<NumberEntity> GetBlacklisted() =>
		dbContext.Numbers.Where(x => x.IsBlacklisted).ToList();

	public void InsertBlacklisted(string number) =>
		Insert(new NumberEntity { Number = number, IsBlacklisted = true });

	public void RemoveBlacklisted(string number)
	{
		var entity = GetOrDefault(number);
		if (entity == null)
			throw new LSCoreNotFoundException();
		entity.IsBlacklisted = false;
		Update(entity);
	}

	public bool IsBlacklisted(string number) =>
		dbContext.Numbers.FirstOrDefault(x => x.Number == number) is { IsBlacklisted: true };
}
