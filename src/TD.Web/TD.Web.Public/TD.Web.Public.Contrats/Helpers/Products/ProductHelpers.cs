using Newtonsoft.Json;
using TD.Web.Public.Contracts.Requests.Products;

namespace TD.Web.Public.Contracts.Helpers.Products;

public static class ProductHelpers
{
    public static string GetProductsCacheKey(ProductsGetRequest request) =>
        JsonConvert.SerializeObject(request);
}
