using System.Text.RegularExpressions;

namespace TD.Web.Admin.Contracts.Helpers.Products;

public static class ProductsHelpers
{
	public static string GenerateSrc(this string name)
	{
		var src = "";
		name = name.Trim(Constants.SrcCharactersToTrim.ToArray());
		src = Regex.Replace(
			name,
			Constants.RegexProductSrcPattern,
			Constants.RegexProductSrcReplacement
		);
		return Regex.Replace(
			src,
			Constants.RegexReplaceMultipleSpacesExpression,
			Constants.RegexReplaceMultipleSpacesReplacement
		);
	}

	public static bool IsSrcNotValid(this string src)
	{
		if (
			src.StartsWith(Constants.RegexReplaceMultipleDashesReplacement)
			|| src.EndsWith(Constants.RegexReplaceMultipleDashesReplacement)
		)
			return true;

		return !Regex.IsMatch(src, Constants.RegexValidateProductSrc);
	}

	public static List<string> FindUnwantedHtmlTags(string input)
	{
		var unwantedTags = new List<string>();
		// style tag is not included cuz it makes problem with <a style="" /> or similar and I want only <style>
		const string pattern =
			@"<(script|meta|head|footer|link|title|iframe|object|embed|applet|form|input|button|textarea|doctype)(\s[^>]*)?>";

		var matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);

		foreach (Match match in matches)
			unwantedTags.Add(match.Value);

		return unwantedTags;
	}
}
