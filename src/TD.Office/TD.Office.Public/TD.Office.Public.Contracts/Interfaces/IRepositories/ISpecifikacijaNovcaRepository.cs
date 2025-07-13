using System.ComponentModel;
using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface ISpecifikacijaNovcaRepository : ILSCoreRepositoryBase<SpecifikacijaNovcaEntity>
{
	SpecifikacijaNovcaEntity? GetCurrentOrDefaultByMagacinId(int magacinId);
	SpecifikacijaNovcaEntity Get(long id);
	SpecifikacijaNovcaEntity GetByDate(DateTime date, int magacinId);
	SpecifikacijaNovcaEntity? GetByDateOrDefault(DateTime date, int magacinId);
	SpecifikacijaNovcaEntity GetNext(
		long relativeToId,
		bool fixMagacin,
		ListSortDirection direction
	);
}
