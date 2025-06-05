using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace TD.Web.Public.Contracts.Helpers.Products;

public static class ProductsHelpers
{
	public static async Task<string> ConvertImageToWebPAsync(byte[] imageData, int quality)
	{
		using var inputStream = new MemoryStream(imageData);
		using var outputStream = new MemoryStream();
		using var image = await Image.LoadAsync(inputStream);

		var webpEncoder = new WebpEncoder { Quality = quality };
		await image.SaveAsync(outputStream, webpEncoder);

		return $"data:image/webp;base64,{Convert.ToBase64String(outputStream.ToArray())}";
	}
}
