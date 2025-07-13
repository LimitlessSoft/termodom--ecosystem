using System.ComponentModel;
using LSCore.Exceptions;
using LSCore.Repository;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class SpecifikacijaNovcaRepository(OfficeDbContext dbContext)
	: LSCoreRepositoryBase<SpecifikacijaNovcaEntity>(dbContext),
		ISpecifikacijaNovcaRepository
{
	public SpecifikacijaNovcaEntity GetByDate(DateTime date, int magacinId)
	{
		var entity = GetByDateOrDefault(date, magacinId);
		if (entity == null)
			throw new LSCoreNotFoundException();

		return entity;
	}

	public SpecifikacijaNovcaEntity? GetByDateOrDefault(DateTime date, int magacinId)
	{
		return dbContext.SpecifikacijeNovca.FirstOrDefault(x =>
			x.IsActive && x.MagacinId == magacinId && x.Datum.Date == date.Date
		);
	}

	/// <summary>
	/// Retrieves the active <see cref="SpecifikacijaNovcaEntity"/> for the specified warehouse
	/// based on the current UTC date. If no matching entity is found, returns null.
	/// </summary>
	/// <param name="magacinId"></param>
	/// <returns></returns>
	public SpecifikacijaNovcaEntity? GetCurrentOrDefaultByMagacinId(int magacinId) =>
		dbContext.SpecifikacijeNovca.FirstOrDefault(x =>
			x.CreatedAt.Date == DateTime.UtcNow.Date && x.IsActive && x.MagacinId == magacinId
		);

	/// <summary>
	/// Returns the next <see cref="SpecifikacijaNovcaEntity"/>.
	/// </summary>
	/// <param name="relativeToId">Search for next <see cref="SpecifikacijaNovcaEntity"/> will start from existing <see cref="SpecifikacijaNovcaEntity"/> with this id</param>
	/// <param name="fixMagacin">If set to true, search will consist only of <see cref="SpecifikacijaNovcaEntity"/> with same <see cref="SpecifikacijaNovcaEntity.MagacinId"/></param>
	/// <param name="direction">If set to Ascending, it will look for greater <see cref="SpecifikacijaNovcaEntity.Id"/>, otherwise it will look for smaller one.</param>
	/// <returns></returns>
	/// <exception cref="LSCoreNotFoundException"></exception>
	public SpecifikacijaNovcaEntity GetNext(
		long relativeToId,
		bool fixMagacin,
		ListSortDirection direction
	)
	{
		var relativeTo = dbContext.SpecifikacijeNovca.FirstOrDefault(x => x.IsActive && x.Id == relativeToId);
		if (relativeTo == null)
			throw new LSCoreNotFoundException();
		
		var query = dbContext.SpecifikacijeNovca.Where(x => x.IsActive);
		if (fixMagacin) {
			query = query.Where(x => x.MagacinId == relativeTo.MagacinId);
			if(direction == ListSortDirection.Ascending)
				query = query.Where(x => x.Datum > relativeTo.Datum).OrderBy(x => x.Datum);
			else
				query = query.Where(x => x.Datum < relativeTo.Datum).OrderByDescending(x => x.Datum);
		}
		else {
			if(direction == ListSortDirection.Ascending)
				query = query.Where(x => x.Id > relativeToId).OrderBy(x => x.Id);
			else
				query = query.Where(x => x.Id < relativeToId).OrderByDescending(x => x.Id);
		}
		
		var entity = query.FirstOrDefault();
		if (entity == null)
			throw new LSCoreNotFoundException();

		return entity;
	}
}
