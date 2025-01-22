using System.ComponentModel;
using LSCore.Contracts.Exceptions;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class SpecifikacijaNovcaRepository(OfficeDbContext dbContext) : ISpecifikacijaNovcaRepository
{
    public SpecifikacijaNovcaEntity GetByDate(DateTime date, int magacinId)
    {
        var entity = dbContext.SpecifikacijeNovca.FirstOrDefault(x =>
            x.IsActive
            && x.MagacinId == magacinId
            && x.CreatedAt >= date // compare only date parts if we are comparing dates, no need to do additional math by adding 1 day
            && x.CreatedAt < date.AddDays(1) // compare only date parts if we are comparing dates, no need to do additional math by adding 1 day
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
    /// Explain what this method does since it's not clear from the name.
    /// </summary>
    /// <param name="magacinId"></param>
    /// <returns></returns>
    public SpecifikacijaNovcaEntity? GetCurrentOrDefault(int magacinId) =>
        dbContext.SpecifikacijeNovca.FirstOrDefault(x =>
            // For two lines bellow regarding DateTime.Today:
            // Is this UTC+0 or local client time or server time?
            // If we have scenario like this:
            // Client1 UTC+2 current time: 05.05.2025. 13:00:00
            // Client2 UTC-12 current time: 04.05.2025. 23:00:00
            // Server UTC+14 current time: 06.05.2025. 03:00:00
            // UTC+0 current time: 05.05.2025. 11:00:00

            // If client1 calls this method, will he get data for 05.05 (his time)
            // If client2 calls this method, will he get data for 04.05 (his time)
            // Or they will both get data for 06.05 (server time)?
            // Or they will both get data for 05.05 (UTC+0 time)?
            x.CreatedAt >= DateTime.Today // compare only date parts if we are comparing dates, no need to do additional math by adding 1 day
            && x.CreatedAt < DateTime.Today.AddDays(1) // compare only date parts if we are comparing dates, no need to do additional math by adding 1 day
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
