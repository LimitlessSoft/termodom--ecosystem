using System.ComponentModel;
using LSCore.Contracts.Exceptions;
using LSCore.Repository;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class SpecifikacijaNovcaRepository(OfficeDbContext dbContext)
    : LSCoreRepositoryBase<SpecifikacijaNovcaEntity>(dbContext), ISpecifikacijaNovcaRepository
{
    public SpecifikacijaNovcaEntity GetByDate(DateTime date, int magacinId)
    {
        var entity = dbContext.SpecifikacijeNovca.FirstOrDefault(x =>
            x.IsActive
            && x.MagacinId == magacinId
            && x.CreatedAt.Date == date.Date
        );

        if (entity == null)
            throw new LSCoreNotFoundException();

        return entity;
    }

    public SpecifikacijaNovcaEntity Get(long id)
    {
        var entity = dbContext.SpecifikacijeNovca.FirstOrDefault(x => x.Id == id && x.IsActive);

        if (entity == null)
            throw new LSCoreNotFoundException();

        return entity;
    }

    /// <summary>
    /// Retrieves the active <see cref="SpecifikacijaNovcaEntity"/> for the specified warehouse
    /// based on the current UTC date. If no matching entity is found, returns null.
    /// </summary>
    /// <param name="magacinId"></param>
    /// <returns></returns>
    public SpecifikacijaNovcaEntity? GetCurrentOrDefault(int magacinId) =>
        dbContext.SpecifikacijeNovca.FirstOrDefault(x =>
            x.CreatedAt.Date == DateTime.UtcNow.Date
            && x.IsActive
            && x.MagacinId == magacinId
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
        var query = dbContext.SpecifikacijeNovca.Where(x =>
            x.IsActive
            && (
                fixMagacin == false
                || x.MagacinId
                    == dbContext
                        .SpecifikacijeNovca.First(z => z.IsActive && z.Id == relativeToId)
                        .MagacinId
            )
        );

        var entity =
            direction == ListSortDirection.Ascending
                ? query.OrderBy(x => x.Id).FirstOrDefault(x => x.Id > relativeToId)
                : query.OrderByDescending(x => x.Id).FirstOrDefault(x => x.Id < relativeToId);

        if (entity == null)
            throw new LSCoreNotFoundException();

        return entity;
    }
}
