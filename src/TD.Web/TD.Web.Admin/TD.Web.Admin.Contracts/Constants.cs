namespace TD.Web.Admin.Contracts
{
	public static class Constants
	{
		public static readonly string RegexProductSrcPattern =
			"[!@#$%^&*()_+`~:'\"\\\\|=/?.<>\\[\\]{}-]";

		public static readonly string RegexProductSrcReplacement = string.Empty;

		public static readonly string SrcCharactersToTrim = " -";

		public static readonly string RegexReplaceMultipleSpacesExpression = " +";

		public static readonly string RegexReplaceMultipleSpacesReplacement = "-";

		public static readonly string RegexReplaceMultipleDashesPattern = @"-+";

		public static readonly string RegexReplaceMultipleDashesReplacement = "-";

		public static readonly string RegexValidateProductSrc = "^[a-zA-Z0-9-čćžđšČĆŽĐŠ]*$";

		public static readonly string RegexValidateUnitName = "^[a-zA-Z0-9]*$";

		public static readonly string RegexValidateAltValuePattern = "[@!#$%^&*()]";
		public static readonly string KomercijalnoApiUrlFormat =
			"https://{0}-komercijalno.termodom.rs";
		public static readonly string DefaultOrderUnlinkFromKomercijalnoKomentar = "TD-SAJT";
		public static readonly List<string> SearchPhrasesStatisticsExclude = new List<string>
		{
			"za",
			"td",
		};
	}
}
