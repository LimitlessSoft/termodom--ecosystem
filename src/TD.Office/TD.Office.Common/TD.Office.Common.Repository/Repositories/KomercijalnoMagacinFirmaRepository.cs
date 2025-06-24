using LSCore.Exceptions;
using LSCore.Repository;
using LSCore.Repository.Contracts;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.IRepositories;

namespace TD.Office.Common.Repository.Repositories;

public class KomercijalnoMagacinFirmaRepository(
	OfficeDbContext dbContext,
	ILogger<KomercijalnoMagacinFirmaRepository> logger
)
	: LSCoreRepositoryBase<KomercijalnoMagacinFirmaEntity>(dbContext),
		IKomercijalnoMagacinFirmaRepository
{
	public KomercijalnoMagacinFirmaEntity GetByMagacinId(int polazniMagacinMagacinId)
	{
		var entity = GetBymagacinIdOrDefault(polazniMagacinMagacinId);
		if (entity != null)
			return entity;
		logger.LogError(
			"Magacin sa ID {MagacinId} nije povezan sa firmom u office bazi tabela KomercijalnoMagacinFirma.",
			polazniMagacinMagacinId
		);
		throw new LSCoreNotFoundException();
	}

	public KomercijalnoMagacinFirmaEntity? GetBymagacinIdOrDefault(int polazniMagacinMagacinId) =>
		GetMultiple().FirstOrDefault(x => x.MagacinId == polazniMagacinMagacinId);
}
