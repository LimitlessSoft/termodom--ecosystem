using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Dtos.ProductPrices;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.ProductPrices;

namespace TD.Web.Public.Domain.Managers
{
    public class ProductPriceManager : LSCoreBaseManager<ProductPriceManager, ProductPriceEntity>, IProductPriceManager
    {
        public ProductPriceManager(ILogger<ProductPriceManager> logger, WebDbContext dbContext)
           : base(logger, dbContext)
        {
        }
        public LSCoreResponse<GetProductPricesDto> GetProductPrice(GetProductPriceRequest request)
        {
            return First<ProductPriceEntity, GetProductPricesDto>(x => x.ProductId == request.ProductId && x.IsActive);
        }
    }
}
