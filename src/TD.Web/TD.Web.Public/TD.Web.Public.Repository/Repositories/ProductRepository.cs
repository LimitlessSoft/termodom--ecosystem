using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Interfaces.Repositories;

namespace TD.Web.Public.Repository.Repositories;

public class ProductRepository(WebDbContext dbContext) : IProductRepository
{
    public Task<Dictionary<long, ProductEntity>> GetAllAsDictionaryAsync() =>
        dbContext
            .Products.Include(x => x.Price)
            .Where(x => x.IsActive)
            .ToDictionaryAsync(x => x.Id, x => x);
}
