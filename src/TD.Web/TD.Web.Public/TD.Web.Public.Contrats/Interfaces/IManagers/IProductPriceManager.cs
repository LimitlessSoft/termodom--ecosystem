using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Web.Public.Contracts.Dtos.ProductPrices;
using TD.Web.Public.Contracts.Requests.ProductPrices;

namespace TD.Web.Public.Contracts.Interfaces.IManagers
{
    public interface IProductPriceManager : ILSCoreBaseManager
    {
        public LSCoreResponse<GetProductPricesDto> GetProductPrice(GetProductPriceRequest request);
    }
}
