using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Web.Admin.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Admin.Contracts.Requests.ProductPriceGroup;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IProductPriceGroupManager
    {
        public Response<long> Save(ProductPriceGroupSaveRequest request);
        public ListResponse<ProductPriceGroupGetDto> GetMultiple();
        public Response Delete(IdRequest request);
    }
}
