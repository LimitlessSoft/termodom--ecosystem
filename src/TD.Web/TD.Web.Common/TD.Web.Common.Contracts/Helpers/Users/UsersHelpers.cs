using System.Text.RegularExpressions;

namespace TD.Web.Common.Contracts.Helpers.Users
{
    public static class UsersHelpers
    {
        public static bool IsUsernameNotValid(this string username) =>
            !Regex.IsMatch(username, Constants.RegexValidateUsernamePattern);
        
        public static bool IsPasswordNotStrong(this string password) =>
            !Regex.IsMatch(password, Constants.RegexValidatePasswordPattern);
    }
}
