using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using LSCore.Contracts.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace TD.Office.Public.Repository.Repositories;
public class SpecifikacijaNovcaRepository(OfficeDbContext dbContext)
    : ISpecifikacijaNovcaRepository
{
    public SpecifikacijaNovcaEntity GetByDate(DateTime date, int magacinId)
    {
        var entity = dbContext.SpecifikacijeNovca
            .Where(x => x.IsActive && x.MagacinId == magacinId && x.CreatedAt >= date && x.CreatedAt < date.AddDays(1)).FirstOrDefault();

        if (entity == null)
            throw new LSCoreNotFoundException();

        return entity;
    }

    public SpecifikacijaNovcaEntity GetById(long id)
    {
        var entity = dbContext.SpecifikacijeNovca
            .Where(x => x.Id == id && x.IsActive).FirstOrDefault();

        if (entity == null)
            throw new LSCoreNotFoundException();

        return entity;
    }

    public SpecifikacijaNovcaEntity? GetCurrent(int magacinId) =>
        dbContext
            .SpecifikacijeNovca
            .Where(x => x.CreatedAt >= DateTime.Today && x.CreatedAt < DateTime.Today.AddDays(1)
                && x.IsActive && x.MagacinId == magacinId
             )
            .FirstOrDefault();

    public SpecifikacijaNovcaEntity GetNextOrPrevious(long relativeToId, bool fixMagacin, bool isNext)
    {
        var query = dbContext.SpecifikacijeNovca
            .Where(x => x.IsActive);

        query = isNext ? 
            query.Where(x => x.Id > relativeToId) : 
            query.Where(x => x.Id < relativeToId);

        if (fixMagacin)
        {
            var magacinId = dbContext.SpecifikacijeNovca
                .Where(x => x.Id == relativeToId)
                .Select(x => x.MagacinId)
                .FirstOrDefault();

            query = query.Where(x => x.MagacinId == magacinId);
        }
        query = isNext ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt);

        var entity = query.FirstOrDefault();

        if (entity == null)
            throw new LSCoreNotFoundException();

        return entity;
    }

}
