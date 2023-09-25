using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Web.Contracts.Dtos.ProductPrices;

namespace TD.Web.Contracts.Interfaces.IManagers
{
    public interface IProductPriceManager
    {
        ListResponse<ProductsPricesGetDto> GetMultiple();
        Response<bool> Delete(IdRequest id);
    }
}
