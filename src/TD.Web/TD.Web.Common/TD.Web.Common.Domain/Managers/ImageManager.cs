using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Images;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Exceptions;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using LSCore.Contracts.Dtos;
using LSCore.Contracts;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using TD.Web.Common.Contracts.Dtos;

namespace TD.Web.Common.Domain.Managers;

public class ImageManager (ILogger<ImageManager> logger, IMinioManager minioManager)
    : LSCoreManagerBase<ImageManager>(logger), IImageManager
{
    public async Task<string> UploadAsync(ImagesUploadRequest request)
    {
        request.Validate();
    
        var uploadedFileName = String.Empty;
        var extension = Path.GetExtension(request.Image.FileName);

        await using (var stream = request.Image.OpenReadStream()) 
        {
            var hashCreator = SHA256.Create();
            var tags = new Dictionary<string, string>()
            {
                { Contracts.Constants.AltTextTag, request.AltText ?? String.Empty }
            };
            
            var hash = hashCreator.ComputeHash(Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString(Contracts.Constants.UploadImageFileNameDateTimeFormatString)));
            foreach (byte c in hash)
                uploadedFileName += $"{c:X2}";
    
            uploadedFileName = Path.ChangeExtension(uploadedFileName, extension);
    
            await minioManager.UploadAsync(stream, Path.Combine(Contracts.Constants.DefaultImageFolderPath, uploadedFileName), request.Image.ContentType, tags);
        }
    
        return uploadedFileName;
    }
    
    public async Task<FileDto> GetImageAsync(ImagesGetRequest request)
    {
        request.Validate();
    
        var imageResponse = await minioManager.DownloadAsync(Path.Combine(Contracts.Constants.DefaultImageFolderPath, request.Image));
    
        using var ms = new MemoryStream(imageResponse.Data!);
        using var img = await Image.LoadAsync(ms);
    
        var K = (double)Math.Max(img.Width, img.Height) / (double)request.Quality;
        img.Mutate(x => x.Resize((int)(img.Width / K), (int)(img.Height / K)));
    
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