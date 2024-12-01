namespace TD.Web.Common.Contracts.Helpers
{
    public static class MobilePhoneHelpers
    {
        public static string GenarateValidNumber(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return mobile;
            
            var newMobile = new string(mobile.Where(t => char.IsDigit(t) || t.Equals('+')).ToArray());
            if (newMobile.IndexOf('0') == 0)
                newMobile = "+381" + newMobile.Substring(1);
            
            if (newMobile.IndexOf('+') == 0 && newMobile[4] == '0')
                newMobile = newMobile.Remove(4, 1);
            
            return newMobile[..Math.Min(16, newMobile.Length)];
        }

        /// <summary>
        /// Checks if the mobile phone number is valid.
        /// Run this after <see cref="GenarateValidNumber"/> method.
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsValidMobile(string mobile) =>
            !string.IsNullOrWhiteSpace(mobile) && mobile.Length is >= 12 and <= 16;
    }
}