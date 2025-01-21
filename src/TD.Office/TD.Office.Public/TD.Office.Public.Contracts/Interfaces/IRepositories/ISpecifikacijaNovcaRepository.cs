using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;
public interface ISpecifikacijaNovcaRepository
{
    SpecifikacijaNovcaEntity? GetCurrent(int magacinId);
    SpecifikacijaNovcaEntity GetById(long id);
    SpecifikacijaNovcaEntity GetByDate(DateTime? date, int magacinId);
    SpecifikacijaNovcaEntity GetNextOrPrevious(long relativeToId, bool fixMagacin, bool isNext);
}
