namespace TD.Web.Common.Contracts.Helpers
{
    public static class MobilePhoneHelpers
    {
        public static string GenarateValidNumber(string mobile)
        {
            string newMobile = new string(mobile.Where(t => char.IsDigit(t) || t.Equals('+')).ToArray());
            if (newMobile.IndexOf('0') == 0)
            {
                newMobile = "+381" + newMobile.Substring(1);
            }
            if (newMobile.IndexOf('+') == 0 && newMobile[4] == '0')
                newMobile = newMobile.Remove(4, 1);
            return newMobile;
        }
    }
}