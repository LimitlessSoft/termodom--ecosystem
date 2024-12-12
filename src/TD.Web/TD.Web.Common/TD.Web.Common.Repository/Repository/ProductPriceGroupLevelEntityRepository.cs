using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Common.Repository.Repository;

public class ProductPriceGroupLevelEntityRepository(IWebDbContextFactory dbContextFactory)
    : IProductPriceGroupLevelEntityRepository
{
    public Dictionary<long, ProductPriceGroupLevelEntity> GetByUserId(long requestUserId)
    {
        var context = dbContextFactory.Create<WebDbContext>();
        return context!
            .ProductPriceGroupLevel.Where(x => x.IsActive && x.UserId == requestUserId)
            .ToDictionary(x => x.ProductPriceGroupId, x => x);
    }
}
