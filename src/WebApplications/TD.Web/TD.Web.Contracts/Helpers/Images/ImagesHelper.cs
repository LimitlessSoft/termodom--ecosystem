using System.Text.RegularExpressions;

namespace TD.Web.Contracts.Helpers.Images
{
    public static class ImagesHelper
    {
        public static bool IsImageTypeNotValid(this string name)
        {
            if (name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                name.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                name.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }

        public static bool isAltValueNotValid(this string alt)
        {
            return !Regex.IsMatch(alt,Constants.RegexValidateAltValuePattern);
        }
    }
}
