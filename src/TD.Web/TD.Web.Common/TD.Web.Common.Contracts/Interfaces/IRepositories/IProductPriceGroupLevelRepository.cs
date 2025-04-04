using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Interfaces.IRepositories;

public interface IProductPriceGroupLevelRepository
{
	Dictionary<long, ProductPriceGroupLevelEntity> GetByUserId(long requestUserId);
	IQueryable<ProductPriceGroupLevelEntity> GetMultipleAsQueryable(IWebDbContext dbContext);
	void Insert(ProductPriceGroupLevelEntity productPriceGroupLevelEntity);
	void Update(ProductPriceGroupLevelEntity priceLevel);
}
