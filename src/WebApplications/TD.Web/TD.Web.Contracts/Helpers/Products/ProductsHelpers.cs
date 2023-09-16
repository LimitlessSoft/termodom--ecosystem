using System.Text.RegularExpressions;

namespace TD.Web.Contracts.Helpers.Products
{
    public static class ProductsHelpers
    {
        public static string GenerateSrc(this string name)
        {
            var src = "";
            src = Regex.Replace(name, Constants.RegexReplaceMultipleSpacesExpression, Constants.RegexReplaceMultipleSpacesReplacement);
            return src;
        }

        public static bool ValidateSrc(this string src)
        {
            throw new NotImplementedException();
        }
    }
}
