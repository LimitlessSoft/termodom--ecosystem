using TD.Core.Contracts.Http;
using TD.Web.Veleprodaja.Contracts.Dtos.Products;
using TD.Web.Veleprodaja.Contracts.Requests.Products;

namespace TD.Web.Veleprodaja.Contracts.IManagers
{
    public interface IProductManager
    {
        ListResponse<ProductsGetDto> GetMultiple();
        Response<ProductsGetDto> Put(ProductsPutRequest request);
    }
}
