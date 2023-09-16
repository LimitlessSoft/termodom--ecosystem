namespace TD.Web.Contracts
{
    public static class Constants
    {
        /// <summary>
        /// Pattern for special characters
        /// </summary>
        public static string RegexProductSrcPattern = "[!@#$%^&*()_+`~:'\"\\\\|=/?.<>\\[\\]{}-]";

        /// <summary>
        /// Replacement for special characters in src
        /// </summary>
        public static string RegexProductSrcReplacement = "";
        /// <summary>
        /// Regex expression for multiple spaces
        /// </summary>
        public static string RegexReplaceMultipleSpacesExpression = " +";

        /// <summary>
        /// Replacement for multiple spaces with '-'
        /// </summary>
        public static string RegexReplaceMultipleSpacesReplacement = "-";

        /// <summary>
        /// Regex patern from for multiple dashes
        /// </summary>
        public static string RegexReplaceMultipleDashesPattern = @"-+";

        /// <summary>
        /// Replacement for multiple dashes
        /// </summary>
        public static string RegexReplaceMultipleDashesReplacement = "-";

        /// <summary>
        /// Pattern for src validation
        /// </summary>
        public static string RegexValidateSrc = "^[a-zA-Z0-9-čćžđšČĆŽĐŠ]*$";
    }
}
