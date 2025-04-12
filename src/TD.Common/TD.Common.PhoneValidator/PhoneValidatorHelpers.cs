namespace TD.Common.PhoneValidator;

public static class PhoneValidatorHelpers
{
	/// <summary>
	/// Validates country code. If invalid - throws exception
	/// </summary>
	/// <param name="countryCode"></param>
	/// <exception cref="ArgumentException"></exception>
	/// <exception cref="Exception"></exception>
	internal static void ValidateCountryCode(string countryCode)
	{
		if (string.IsNullOrWhiteSpace(countryCode))
			throw new ArgumentException("Country code must be provided", nameof(countryCode));

		if (countryCode[0] == '+')
			throw new Exception("Invalid country code. Add country code without +");
	}
}
