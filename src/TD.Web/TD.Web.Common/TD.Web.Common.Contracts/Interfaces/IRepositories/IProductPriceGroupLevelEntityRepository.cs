using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Interfaces.IRepositories;

public interface IProductPriceGroupLevelEntityRepository
{
    Dictionary<long, ProductPriceGroupLevelEntity> GetByUserId(long requestUserId);
}
