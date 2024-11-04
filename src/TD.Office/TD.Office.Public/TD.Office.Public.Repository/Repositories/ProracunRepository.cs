using LSCore.Contracts.Exceptions;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class ProracunRepository(OfficeDbContext dbContext) : IProracunRepository
{
    // <inheritdoc />
    public ProracunEntity Get(long id)
    {
        var p = dbContext
            .Proracuni.Include(x => x.Items)
            .FirstOrDefault(x => x.Id == id && x.IsActive);

        if (p == null)
            throw new LSCoreNotFoundException();

        return p;
    }
}
