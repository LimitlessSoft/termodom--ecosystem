using BCrypt.Net;

namespace TD.Backuper.Common.Contracts.Helpers
{
    public static class KeyHelpers
    {
        public static string GenerateKey()
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(Constants.KeyBase + DateTime.Now.ToString("dd-MM-yyyy"));
        }

        public static bool VerifyKey(string key)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(Constants.KeyBase + DateTime.Now.ToString("dd-MM-yyyy"), key);
        }
    }
}
