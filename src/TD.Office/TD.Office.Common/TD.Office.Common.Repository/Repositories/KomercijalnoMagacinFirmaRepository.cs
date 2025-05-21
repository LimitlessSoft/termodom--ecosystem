using LSCore.Exceptions;
using LSCore.Repository;
using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.IRepositories;

namespace TD.Office.Common.Repository.Repositories;

public class KomercijalnoMagacinFirmaRepository(OfficeDbContext dbContext)
	: LSCoreRepositoryBase<KomercijalnoMagacinFirmaEntity>(dbContext),
		IKomercijalnoMagacinFirmaRepository
{
	public KomercijalnoMagacinFirmaEntity GetByMagacinId(int polazniMagacinMagacinId)
	{
		var entity = GetBymagacinIdOrDefault(polazniMagacinMagacinId);
		if (entity == null)
			throw new LSCoreNotFoundException();
		return entity;
	}

	public KomercijalnoMagacinFirmaEntity? GetBymagacinIdOrDefault(int polazniMagacinMagacinId) =>
		GetMultiple().FirstOrDefault(x => x.MagacinId == polazniMagacinMagacinId);
}
