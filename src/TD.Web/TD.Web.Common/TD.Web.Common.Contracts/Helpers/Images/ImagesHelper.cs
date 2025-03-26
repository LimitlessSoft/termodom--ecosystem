using System.Text.RegularExpressions;

// using Microsoft.AspNetCore.Http;

namespace TD.Web.Common.Contracts.Helpers.Images
{
	public static class ImagesHelper
	{
		// public static bool IsImageTypeNotValid(this IFormFile file)
		// {
		// 	try
		// 	{
		// 		if (
		// 			!file.FileName.Contains(".jpg", StringComparison.CurrentCultureIgnoreCase)
		// 			&& !file.FileName.Contains(".jpeg", StringComparison.CurrentCultureIgnoreCase)
		// 			&& !file.FileName.Contains(".png", StringComparison.CurrentCultureIgnoreCase)
		// 		)
		// 			return true;
		//
		// 		using var br = new BinaryReader(file.OpenReadStream());
		// 		var soi = br.ReadUInt16();
		// 		var marker = br.ReadUInt16();
		//
		// 		var isJpeg = soi == 0xd8ff && (marker & 0xe0ff) == 0xe0ff;
		// 		var isPng = soi == 0x5089;
		//
		// 		return !(isJpeg || isPng);
		// 	}
		// 	catch
		// 	{
		// 		return true;
		// 	}
		// }

		// public static bool isAltValueNotValid(this string alt) =>
		// 	Regex.IsMatch(alt, Constants.RegexValidateAltValuePattern);
	}
}
