using LSCore.Contracts.Exceptions;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;

namespace TD.Komercijalno.Repository.Repositories;

public class MagacinRepository(KomercijalnoDbContext dbContext) : IMagacinRepository
{
    public Magacin Get(short id)
    {
        var entity = GetOrDefault(id);
        if (entity == null)
            throw new LSCoreNotFoundException();

        return entity;
    }

    public Magacin? GetOrDefault(short id) =>
        dbContext.Magacini.FirstOrDefault(x => x.Id == id);
}