using System.Text.RegularExpressions;

namespace TD.Web.Admin.Contracts.Helpers.Units
{
	public static class UnitsHelpers
	{
		public static bool IsNameNotValid(this string name)
		{
			return !Regex.IsMatch(name, Constants.RegexValidateUnitName);
		}

		public static string NormalizeName(this string name)
		{
			return name.ToUpper();
		}
	}
}
