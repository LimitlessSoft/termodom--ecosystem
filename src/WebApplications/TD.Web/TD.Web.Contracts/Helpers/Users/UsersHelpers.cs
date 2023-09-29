using System.Text.RegularExpressions;

namespace TD.Web.Contracts.Helpers.Users
{
    public static class UsersHelpers
    {
        public static bool IsUsernameNotValid(this string username)
        {
            return !Regex.IsMatch(username, Constants.RegexValidateUsernamePattern);
        }
        public static bool IsPasswordNotStrong(this string password)
        {
            return !Regex.IsMatch(password, Constants.RegexValidatePasswordPattern);
        }
    }
}
