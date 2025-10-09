namespace TD.Web.Common.Contracts.Helpers
{
	public static class PricesHelpers
	{
		public static decimal CalculatePriceK(decimal minPrice, decimal maxPrice) =>
			(maxPrice - minPrice) * LegacyConstants.DiscountPartFromDifference;

		public static int CalculateCartLevel(decimal totalCartValueWithoutVAT) =>
			(int)
				Math.Min(
					Math.Floor(
						totalCartValueWithoutVAT
							/ (
								LegacyConstants.MaximumCartValueForDiscount
								/ LegacyConstants.NumberOfCartValueStages
							)
					),
					LegacyConstants.NumberOfCartValueStages
				);

		public static decimal CalculateOneTimeCartPrice(
			decimal minPrice,
			decimal maxPrice,
			decimal totalCartValueWithoutVAT
		) =>
			maxPrice
			- (
				CalculatePriceK(minPrice, maxPrice)
				/ LegacyConstants.NumberOfCartValueStages
				* CalculateCartLevel(totalCartValueWithoutVAT)
			);

		public static decimal CalculateProductPriceByLevel(
			decimal minPrice,
			decimal maxPrice,
			int level,
			decimal? totalCartValueWithoutVAT = null
		) {
			if(totalCartValueWithoutVAT is not null && totalCartValueWithoutVAT > LegacyConstants.MaximumCartValueForDiscount)
				return CalculateOneTimeCartPrice(minPrice, maxPrice, totalCartValueWithoutVAT.Value);
			var profiPrice = maxPrice
				- (
				(
				CalculatePriceK(minPrice, maxPrice)
				/ (LegacyConstants.NumberOfProductPriceGroupLevels - 1)
				) * level
				);

			return profiPrice;
		}

		public static decimal? CalculateValueToNextLevel(decimal totalCartValueWithoutVAT) =>
			(
				CalculateCartLevel(totalCartValueWithoutVAT) + 1
				<= LegacyConstants.NumberOfCartValueStages
			)
				? (CalculateCartLevel(totalCartValueWithoutVAT) + 1)
					* (
						LegacyConstants.MaximumCartValueForDiscount
						/ LegacyConstants.NumberOfCartValueStages
					)
					- totalCartValueWithoutVAT
				: null;
	}
}
