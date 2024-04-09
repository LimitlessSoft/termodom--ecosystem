using System.Text;
using System.Text.RegularExpressions;

namespace TD.Web.Common.Contracts.Helpers.Users
{
    public static class UsersHelpers
    {
        public static bool IsUsernameNotValid(this string username) =>
            !Regex.IsMatch(username, Constants.RegexValidateUsernamePattern);
        
        public static bool IsPasswordNotStrong(this string password) =>
            !Regex.IsMatch(password, Constants.RegexValidatePasswordPattern);
        
        public static string GenerateNewPassword()
        {
            int length = 6;
            const string valid = "abcABC1234567890";
            var res = new StringBuilder();
            while (0 < length--)
                res.Append(valid[Random.Shared.Next(valid.Length)]);
            res.Append("cH1");
            return res.ToString();
        }
    }
}
