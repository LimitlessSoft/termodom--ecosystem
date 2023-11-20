using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Requests;
using TD.Web.Admin.Contracts.Dtos.ProductPrices;
using TD.Web.Admin.Contracts.Requests.ProductsPrices;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IProductPriceManager : ILSCoreBaseManager
    {
        LSCoreListResponse<ProductsPricesGetDto> GetMultiple();
        LSCoreResponse Delete(LSCoreIdRequest id);
        LSCoreResponse<long> Save(SaveProductPriceRequest request);
    }
}
