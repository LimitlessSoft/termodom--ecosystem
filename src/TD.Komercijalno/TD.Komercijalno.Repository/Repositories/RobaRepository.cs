using System.Linq.Expressions;
using LSCore.Contracts.Exceptions;
using Microsoft.EntityFrameworkCore;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;

namespace TD.Komercijalno.Repository.Repositories;

public class RobaRepository(KomercijalnoDbContext dbContext) : IRobaRepository
{
    public Roba Get(int id, params Expression<Func<Roba, object>>[] includes)
    {
        var entity = GetOrDefault(id, includes);
        if (entity == null)
            throw new LSCoreNotFoundException();

        return entity;
    }

    public Roba? GetOrDefault(int id, params Expression<Func<Roba, object>>[] includes) =>
        includes.Aggregate(dbContext.Roba.AsQueryable(),
            (current, include) => current.Include(include))
            .FirstOrDefault(x => x.Id == id);
}