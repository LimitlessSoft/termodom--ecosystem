namespace TD.Web.Contracts
{
    public static class Constants
    {
        public static readonly string RegexProductSrcPattern = "[!@#$%^&*()_+`~:'\"\\\\|=/?.<>\\[\\]{}-]";

        public static readonly string RegexProductSrcReplacement = string.Empty;

        public static readonly string SrcCharactersToTrim = " -";

        public static readonly string RegexReplaceMultipleSpacesExpression = " +";

        public static readonly string RegexReplaceMultipleSpacesReplacement = "-";

        public static readonly string RegexReplaceMultipleDashesPattern = @"-+";

        public static readonly string RegexReplaceMultipleDashesReplacement = "-";

        public static readonly string RegexValidateProductSrc = "^[a-zA-Z0-9-čćžđšČĆŽĐŠ]*$";

        public static readonly string RegexValidateUnitName = "^[a-zA-Z0-9]*$";

        public static readonly string RegexValidateUsernamePattern = "^[0-9A-Za-z]+$";

        public static readonly string RegexValidatePasswordPattern = @"^(?=.*[0-9])(?=.*[A-Za-z])";
    }
}
