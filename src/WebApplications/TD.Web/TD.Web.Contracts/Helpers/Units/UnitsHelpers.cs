using System.Text.RegularExpressions;

namespace TD.Web.Contracts.Helpers.Units
{
    public static class UnitsHelpers
    {
        public static bool IsNameNotValid(this string name)
        {
            return !Regex.IsMatch(name, Constants.RegexValidateUnitName);
        }
    }
}
