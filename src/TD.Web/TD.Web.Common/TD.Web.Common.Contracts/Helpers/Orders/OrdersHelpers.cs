using System.Security.Cryptography;
using System.Text;

namespace TD.Web.Common.Contracts.Helpers.Orders
{
    public static class OrdersHelpers
    {
        public static string GenerateOneTimeHash()
        {
            var oneTimeHash = string.Empty;
            var hashCreator = MD5.Create();
            var hash = hashCreator.ComputeHash(Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString(Constants.UploadImageFileNameDateTimeFormatString)));

            foreach (byte c in hash)
                oneTimeHash += $"{c:X2}";

            return oneTimeHash;
        }
    }
}
