using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Common.Repository.Repository;

public class ProductPriceGroupLevelRepository(IWebDbContextFactory dbContextFactory)
	: IProductPriceGroupLevelRepository
{
	public Dictionary<long, ProductPriceGroupLevelEntity> GetByUserId(long requestUserId)
	{
		using var context = dbContextFactory.Create<WebDbContext>();
		return context!
			.ProductPriceGroupLevel.Where(x => x.IsActive && x.UserId == requestUserId)
			.ToDictionary(x => x.ProductPriceGroupId, x => x);
	}

	public IQueryable<ProductPriceGroupLevelEntity> GetMultipleAsQueryable(
		IWebDbContext dbContext
	) => dbContext.ProductPriceGroupLevel.Where(x => x.IsActive);

	public void Insert(ProductPriceGroupLevelEntity productPriceGroupLevelEntity)
	{
		using var context = dbContextFactory.Create<WebDbContext>();
		context!.ProductPriceGroupLevel.Add(productPriceGroupLevelEntity);
		context.SaveChanges();
	}

	public void Update(ProductPriceGroupLevelEntity priceLevel)
	{
		using var context = dbContextFactory.Create<WebDbContext>();
		context!.ProductPriceGroupLevel.Update(priceLevel);
		context.SaveChanges();
	}
}
