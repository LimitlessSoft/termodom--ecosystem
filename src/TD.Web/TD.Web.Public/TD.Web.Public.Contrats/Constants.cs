using Newtonsoft.Json;
using TD.Web.Public.Contracts.Requests.Products;
using TD.Web.Public.Contracts.Requests.ProductsGroups;

namespace TD.Web.Public.Contracts;

public static class Constants
{
    public static int ProductsCardsImageQuality = 50;

    public static string ProductsCardsImageContentType = "image/webp";

    public static class CacheKeys
    {
        public const string Products = "all-products-dict";

        public static string ProductGroups(ProductsGroupsGetRequest request) =>
            $"product-groups-{JsonConvert.SerializeObject(request)}";

        public static string ProductsPaginated(ProductsGetRequest request) =>
            $"products-{JsonConvert.SerializeObject(request)}";

        public static string UserPriceLevels(long userId) => $"user-price-levels-{userId}";
    }
}
