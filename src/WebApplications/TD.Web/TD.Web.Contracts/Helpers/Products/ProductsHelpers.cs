using System.Text.RegularExpressions;

namespace TD.Web.Contracts.Helpers.Products
{
    public static class ProductsHelpers
    {
        public static string GenerateSrc(this string name)
        {
            var src = "";
            src = Regex.Replace(name, Constants.RegexProductSrcPattern, Constants.RegexProductSrcReplacement);
            src = Regex.Replace(src, Constants.RegexReplaceMultipleSpacesExpression, Constants.RegexReplaceMultipleSpacesReplacement);
            src = Regex.Replace(src, Constants.RegexReplaceMultipleDashesPattern, Constants.RegexReplaceMultipleDashesReplacement);
            src = src.StartsWith(Constants.RegexReplaceMultipleSpacesReplacement) ? src.Substring(1) : src;
            src = src.EndsWith(Constants.RegexReplaceMultipleSpacesReplacement) ? src.Substring(0, src.Length - 1) : src;
            return src;
        }

        public static bool ValidateSrc(this string src)
        {
            return Regex.IsMatch(src, Constants.RegexValidateSrc);
        }
    }
}
