using System.Text.RegularExpressions;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.Helpers.Products
{
    public static class ProductsHelpers
    {
        public static string GenerateSrc(this string name)
        {
            var src = "";
            name = name.Trim(Constants.SrcCharactersToTrim.ToArray());
            src = Regex.Replace(name, Constants.RegexProductSrcPattern, Constants.RegexProductSrcReplacement);
            return Regex.Replace(src, Constants.RegexReplaceMultipleSpacesExpression, Constants.RegexReplaceMultipleSpacesReplacement);
        }

        public static bool IsSrcNotValid(this string src)
        {
            if (src.StartsWith(Constants.RegexReplaceMultipleDashesReplacement) ||
                src.EndsWith(Constants.RegexReplaceMultipleDashesReplacement))
                return true;

            return !Regex.IsMatch(src, Constants.RegexValidateProductSrc);
        }
    }
}
