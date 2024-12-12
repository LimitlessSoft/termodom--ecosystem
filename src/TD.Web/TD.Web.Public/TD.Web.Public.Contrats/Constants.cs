using Newtonsoft.Json;
using TD.Web.Public.Contracts.Requests.Products;

namespace TD.Web.Public.Contracts;

public static class Constants
{
    public static class CacheKeys
    {
        public const string Products = "all-products-dict";

        public static string ProductsPaginated(ProductsGetRequest request) =>
            JsonConvert.SerializeObject(request);

        public static string UserPriceLevels(long userId) => $"user-price-levels-{userId}";
    }
}
