using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;
public interface ISpecifikacijaNovcaRepository
{
    SpecifikacijaNovcaEntity? GetCurrent(int magacinId);
    SpecifikacijaNovcaEntity GetById(long id);
    SpecifikacijaNovcaEntity GetNextOrPrevious(long relativeToId, bool fixMagacin, bool isNext);
}
