using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Web.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Contracts.Requests.ProductPriceGroup;

namespace TD.Web.Contracts.Interfaces.IManagers
{
    public interface IProductPriceGroupManager
    {
        public Response<long> Save(ProductPriceGroupSaveRequest request);
        public ListResponse<ProductPriceGroupGetDto> GetMultiple();
        public Response<bool> Delete(IdRequest request);
    }
}
