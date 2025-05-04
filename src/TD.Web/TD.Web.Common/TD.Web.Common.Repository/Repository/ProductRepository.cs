using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Common.Repository.Repository;

public class ProductRepository(WebDbContext dbContext)
	: LSCoreRepositoryBase<ProductEntity>(dbContext),
		IProductRepository
{
	public Task<Dictionary<long, ProductEntity>> GetAllAsDictionaryAsync() =>
		GetMultiple().Include(x => x.Price).ToDictionaryAsync(x => x.Id);
}
