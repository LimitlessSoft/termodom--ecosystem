namespace TD.Common.PhoneValidator;

public class PhoneValidator
{
	private readonly string _countryCode;
	private readonly int _maxDigitsCount = 11;
	private readonly int _minDigitsCount = 7;

	/// <summary>
	/// Valid country code without leading + sign
	/// </summary>
	public string CountryCode
	{
		get => _countryCode;
		init
		{
			PhoneValidatorHelpers.ValidateCountryCode(value);
			_countryCode = value;
		}
	}

	/// <summary>
	/// Minimum number of digits in phone number excluding country code
	/// </summary>
	public int MinDigitsCount
	{
		get => _minDigitsCount;
		init => _minDigitsCount = value;
	}

	/// <summary>
	/// Maximum number of digits in phone number excluding country code
	/// </summary>
	public int MaxDigitsCount
	{
		get => _maxDigitsCount;
		init => _maxDigitsCount = value;
	}

	public PhoneValidator(string countryCode) => CountryCode = countryCode;

	public PhoneValidator(string countryCode, int minDigitsCount, int maxDigitsCount)
		: this(countryCode)
	{
		MinDigitsCount = minDigitsCount;
		MaxDigitsCount = maxDigitsCount;
	}

	/// <summary>
	/// Checks if phone number is valid. For phone number to be valid, it needs to follow standard format.
	/// To format phone number, see <see cref="Format(string)"/>
	/// </summary>
	/// <param name="number"></param>
	/// <returns></returns>
	public bool IsValid(string number)
	{
		if (string.IsNullOrWhiteSpace(number))
			return false;
		if (!number.StartsWith(CountryCode))
			return false;
		if (number.Length < MinDigitsCount || number.Length > MaxDigitsCount)
			return false;
		return true;
	}

	/// <summary>
	/// Formats phone number to standard format
	/// </summary>
	/// <param name="number"></param>
	/// <returns></returns>
	public string Format(string number)
	{
		var clean = new string(number.Where(c => char.IsDigit(c) || c == '+').ToArray());

		if (!IsValid(clean))
			throw new ArgumentException("Invalid phone number", nameof(number));

		return clean;
	}
}
