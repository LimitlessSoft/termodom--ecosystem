using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using TD.Web.Admin.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Admin.Contracts.Requests.ProductPriceGroup;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IProductPriceGroupManager
    {
        LSCoreResponse<long> Save(ProductPriceGroupSaveRequest request);
        LSCoreListResponse<ProductPriceGroupGetDto> GetMultiple();
        LSCoreResponse Delete(LSCoreIdRequest request);
    }
}
