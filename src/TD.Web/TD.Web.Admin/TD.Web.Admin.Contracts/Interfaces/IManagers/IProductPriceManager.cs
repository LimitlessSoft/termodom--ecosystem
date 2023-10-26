using TD.Core.Contracts.Http;
using TD.Core.Contracts.IManagers;
using TD.Core.Contracts.Requests;
using TD.Web.Admin.Contracts.Dtos.ProductPrices;
using TD.Web.Admin.Contracts.Requests.ProductsPrices;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IProductPriceManager : IBaseManager
    {
        ListResponse<ProductsPricesGetDto> GetMultiple();
        Response Delete(IdRequest id);
        Response<long> Save(SaveProductPriceRequest request);
    }
}
