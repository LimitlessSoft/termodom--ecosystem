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

            return Regex.Replace(src, Constants.RegexReplaceMultipleDashesPattern, Constants.RegexReplaceMultipleDashesReplacement)
                   .TrimStart(Constants.RegexReplaceMultipleSpacesReplacement.ToCharArray())
                   .TrimEnd(Constants.RegexReplaceMultipleSpacesReplacement.ToCharArray());
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
