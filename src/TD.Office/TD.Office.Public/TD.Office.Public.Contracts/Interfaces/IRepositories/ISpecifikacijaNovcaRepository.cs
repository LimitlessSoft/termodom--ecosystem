using System.ComponentModel;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface ISpecifikacijaNovcaRepository
{
    SpecifikacijaNovcaEntity? GetCurrentOrDefault(int magacinId);
    SpecifikacijaNovcaEntity Get(long id);
    SpecifikacijaNovcaEntity GetByDate(DateTime date, int magacinId);
    SpecifikacijaNovcaEntity GetNext(
        long relativeToId,
        bool fixMagacin,
        ListSortDirection direction
    );
}
