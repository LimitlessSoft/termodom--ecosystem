using System.Text.RegularExpressions;

namespace TD.Web.Contracts.Helpers.Products
{
    public static class ProductsHelpers
    {
        public static string GenerateSrc(this string name)
        {
            var src = "";
            src = Regex.Replace(name, Constants.RegexProductSrcPattern, Constants.RegexProductSrcReplacement)
                       .Replace(Constants.RegexReplaceMultipleSpacesExpression, Constants.RegexReplaceMultipleSpacesReplacement)
                       .Replace(Constants.RegexReplaceMultipleDashesPattern, Constants.RegexReplaceMultipleDashesReplacement);
            src = src.StartsWith(Constants.RegexReplaceMultipleSpacesReplacement) ? src.Substring(1) : src;
            src = src.EndsWith(Constants.RegexReplaceMultipleSpacesReplacement) ? src.Substring(0, src.Length - 1) : src;
            return src;
        }

        public static bool isSrcValid(this string src)
        {
            if (src.StartsWith(Constants.RegexReplaceMultipleDashesReplacement) ||
                src.EndsWith(Constants.RegexReplaceMultipleDashesReplacement)) return false;

            return Regex.IsMatch(src, Constants.RegexValidateProductSrc);
        }

        public static bool isSrcNotValid(this string src)
        {
            if (src.StartsWith(Constants.RegexReplaceMultipleDashesReplacement) ||
                src.EndsWith(Constants.RegexReplaceMultipleDashesReplacement)) return true;

            return !Regex.IsMatch(src, Constants.RegexValidateProductSrc);
        }
    }
}
