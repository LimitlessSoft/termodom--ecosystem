namespace TD.Web.Common.Contracts.Helpers
{
    public static class PricesHelpers
    {
        public static decimal CalculatePriceK(decimal minPrice, decimal maxPrice) => 
            (maxPrice - minPrice) * Constants.DiscountPartFromDifference;

        public static int CalculateCartLevel(decimal totalCartValueWithoutVAT) =>
            (int) Math.Min(Math.Floor(totalCartValueWithoutVAT / (Constants.MaximumCartValueForDiscount / Constants.NumberOfCartValueStages)), Constants.NumberOfCartValueStages);

        public static decimal CalculateOneTimeCartPrice(decimal minPrice, decimal maxPrice, decimal totalCartValueWithoutVAT) =>
            maxPrice - (CalculatePriceK(minPrice, maxPrice) / Constants.NumberOfCartValueStages * CalculateCartLevel(totalCartValueWithoutVAT));

        public static decimal CalculateProductPriceByLevel(decimal minPrice, decimal maxPrice, int level) =>
            maxPrice - ((CalculatePriceK(minPrice, maxPrice) / (Constants.NumberOfProductPriceGroupLevels - 1)) * level);
    }
}
