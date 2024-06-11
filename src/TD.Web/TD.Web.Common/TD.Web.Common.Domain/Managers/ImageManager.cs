using TD.Web.Common.Contracts.Interfaces.IManagers;
using Microsoft.Extensions.Logging;
using LSCore.Domain.Managers;

namespace TD.Web.Common.Domain.Managers;

public class ImageManager (ILogger<ImageManager> logger)
    : LSCoreManagerBase<ImageManager>(logger), IImageManager
{
    // public async Task<string> UploadAsync(ImagesUploadRequest request)
    // {
    //     var response = new LSCoreResponse<string>();
    //
    //     if (request.IsRequestInvalid(response))
    //         return response;
    //
    //     var uploadedFileName = String.Empty;
    //     var extension = Path.GetExtension(request.Image.FileName);
    //
    //     using (Stream stream = request.Image.OpenReadStream()) 
    //     {
    //         var hashCreator = SHA256.Create();
    //         var tags = new Dictionary<string, string>()
    //         {
    //             { Contracts.Constants.AltTextTag, request.AltText ?? String.Empty }
    //         };
    //         
    //         var hash = hashCreator.ComputeHash(Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString(Contracts.Constants.UploadImageFileNameDateTimeFormatString)));
    //         foreach (byte c in hash)
    //             uploadedFileName += $"{c:X2}";
    //
    //         uploadedFileName = Path.ChangeExtension(uploadedFileName, extension);
    //
    //         await _minioManager.UploadAsync(stream, Path.Combine(Contracts.Constants.DefaultImageFolderPath, uploadedFileName), request.Image.ContentType, tags);
    //     }
    //
    //     return new LSCoreResponse<string>(uploadedFileName);
    // }
    //
    // public async Task<LSCoreFileResponse> GetImageAsync(ImagesGetRequest request)
    // {
    //     var response = new LSCoreFileResponse();
    //
    //     if (request.IsRequestInvalid(response))
    //         return response;
    //
    //     var imageResponse = await _minioManager.DownloadAsync(Path.Combine(Contracts.Constants.DefaultImageFolderPath, request.Image));
    //     response.Merge(imageResponse);
    //     if (response.NotOk)
    //         return response;
    //
    //     using var ms = new MemoryStream(imageResponse.Payload.Data);
    //     using var img = Image.Load(ms);
    //
    //     var K = (double)Math.Max(img.Width, img.Height) / (double)request.Quality;
    //     img.Mutate(x => x.Resize((int)(img.Width / K), (int)(img.Height / K)));
    //
    //     var resizedMs = new MemoryStream();
    //     switch(imageResponse.Payload.ContentType)
    //     {
    //         case LSCoreContractsConstants.ImageTypesMIME.Jpeg:
    //             await img.SaveAsJpegAsync(resizedMs);
    //             break;
    //         case LSCoreContractsConstants.ImageTypesMIME.Png:
    //             await img.SaveAsPngAsync(resizedMs);
    //             break;
    //         default:
    //             return LSCoreFileResponse.BadRequest();
    //     }
    //
    //     response.Payload = new LSCore.Contracts.Dtos.LSCoreFileDto()
    //     {
    //         Data = resizedMs.ToArray(),
    //         ContentType = imageResponse.Payload.ContentType,
    //         Tags = imageResponse.Payload.Tags,
    //     };
    //
    //     return response;
    // }
}