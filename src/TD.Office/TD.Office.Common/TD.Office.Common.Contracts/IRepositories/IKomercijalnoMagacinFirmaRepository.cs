using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Contracts.IRepositories;

public interface IKomercijalnoMagacinFirmaRepository
	: ILSCoreRepositoryBase<KomercijalnoMagacinFirmaEntity>
{
	KomercijalnoMagacinFirmaEntity GetByMagacinId(int polazniMagacinMagacinId);
	KomercijalnoMagacinFirmaEntity? GetBymagacinIdOrDefault(int polazniMagacinMagacinId);
}
