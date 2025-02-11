using System.Security.Cryptography;
using System.Text;
using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Domain.Extensions;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using TD.Web.Common.Contracts;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Images;

namespace TD.Web.Common.Domain.Managers;

public class ImageManager (ILogger<ImageManager> logger, IMinioManager minioManager) : IImageManager
{
    public async Task<string> UploadAsync(ImagesUploadRequest request)
    {
        request.Validate();
    
        var uploadedFileName = String.Empty;
        var extension = Path.GetExtension(request.Image.FileName);

        await using (var stream = request.Image.OpenReadStream())
        {
            var tags = new Dictionary<string, string>()
            {
                { Constants.AltTextTag, request.AltText ?? String.Empty }
            };

            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString(Constants.UploadImageFileNameDateTimeFormatString)));
            uploadedFileName = hash.Aggregate(uploadedFileName, (current, c) => current + $"{c:X2}");

            uploadedFileName = Path.ChangeExtension(uploadedFileName, extension);

            await minioManager.UploadAsync(stream, Path.Combine(Constants.DefaultImageFolderPath, uploadedFileName), request.Image.ContentType, tags);
        }

        return uploadedFileName;
    }
    
    public async Task<FileDto> GetImageAsync(ImagesGetRequest request)
    {
        request.Validate();
    
        var imageResponse = await minioManager.DownloadAsync(Path.Combine(Constants.DefaultImageFolderPath, request.Image));
    
        using var ms = new MemoryStream(imageResponse.Data!);
        using var img = await Image.LoadAsync(ms);
    
        var k = Math.Max(img.Width, img.Height) / request.Quality;
        img.Mutate(x => x.Resize((int)(img.Width / k), (int)(img.Height / k)));
    
        var resizedMs = new MemoryStream();
        switch(imageResponse.ContentType)
        {
            case LSCoreContractsConstants.ImageTypesMIME.Jpeg:
                await img.SaveAsJpegAsync(resizedMs);
                break;
            case LSCoreContractsConstants.ImageTypesMIME.Png:
                await img.SaveAsPngAsync(resizedMs);
                break;
            default:
                throw new LSCoreBadRequestException();
        }
    
        return new FileDto
        {
            Data = resizedMs.ToArray(),
            ContentType = imageResponse.ContentType,
            Tags = imageResponse.Tags,
        };
    }
}