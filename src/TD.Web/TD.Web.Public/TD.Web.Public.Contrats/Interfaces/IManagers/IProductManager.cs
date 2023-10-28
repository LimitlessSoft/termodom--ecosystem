using TD.Core.Contracts.Http;
using TD.Core.Contracts.IManagers;
using TD.Web.Public.Contrats.Dtos.Products;
using TD.Web.Public.Contrats.Requests.Products;

namespace TD.Web.Public.Contrats.Interfaces.IManagers
{
    public interface IProductManager : IBaseManager
    {
        ListResponse<ProductsGetDto> GetMultiple(ProductsGetRequest request);
    }
}
