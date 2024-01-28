using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.ProductPriceGroupLevels;

namespace TD.Web.Public.Domain.Managers
{
    public class ProductPriceGroupLevelManager : LSCoreBaseManager<ProductPriceGroupLevelManager, ProductPriceGroupLevelEntity>, IProductPriceGroupLevelManager
    {
        public ProductPriceGroupLevelManager(ILogger<ProductPriceGroupLevelManager> logger, WebDbContext dbContext)
           : base(logger, dbContext)
        {
        }
        public LSCoreResponse<int> GetUserLevel(GetUserLevelRequest request)
        {
            var response = new LSCoreResponse<int>();
            var priceGroupResponse = Queryable(x => x.UserId == request.UserId && x.IsActive && x.ProductPriceGroupId == request.ProductPriceGroupId);
            response.Merge(priceGroupResponse);
            if(response.NotOk)
                return response;
            var priceGroupEntity = priceGroupResponse.Payload!.FirstOrDefault();
            response.Payload = priceGroupEntity?.Level ?? 0;
            return response;
        }
    }
}
