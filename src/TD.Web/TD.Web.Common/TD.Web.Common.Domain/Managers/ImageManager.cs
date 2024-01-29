using System.Text;
using LSCore.Contracts;
using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using LSCore.Domain.Validators;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Extensions;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Requests.Images;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Common.Domain.Managers
{
    public class ImageManager : LSCoreBaseManager<ImageManager>, IImageManager
    {
        private readonly ILSCoreMinioManager _minioManager;
        public ImageManager(ILSCoreMinioManager minioManager, ILogger<ImageManager> logger)
            : base(logger)
        {
            _minioManager = minioManager;
        }

        public async Task<LSCoreResponse<string>> UploadAsync(ImagesUploadRequest request)
        {
            var response = new LSCoreResponse<string>();

            if (request.IsRequestInvalid(response))
                return response;

            var uploadedFileName = String.Empty;
            var extension = Path.GetExtension(request.Image.FileName);

            using (Stream stream = request.Image.OpenReadStream()) 
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

                await _minioManager.UploadAsync(stream, Path.Combine(Contracts.Constants.DefaultImageFolderPath, uploadedFileName), request.Image.ContentType, tags);
            }

            return new LSCoreResponse<string>(uploadedFileName);
        }

        public async Task<LSCoreFileResponse> GetImageAsync(ImagesGetRequest request)
        {
            var response = new LSCoreFileResponse();

            if (request.IsRequestInvalid(response))
                return response;

            var imageResponse = await _minioManager.DownloadAsync(Path.Combine(Contracts.Constants.DefaultImageFolderPath, request.Image));
            response.Merge(imageResponse);
            if (response.NotOk)
                return response;

            using var ms = new MemoryStream(imageResponse.Payload.Data);
            using var img = Image.Load(ms);

            var K = (double)Math.Max(img.Width, img.Height) / (double)request.Quality;
            img.Mutate(x => x.Resize((int)(img.Width / K), (int)(img.Height / K)));

            var resizedMs = new MemoryStream();
            switch(imageResponse.Payload.ContentType)
            {
                case LSCoreContractsConstants.ImageTypesMIME.Jpeg:
                    await img.SaveAsJpegAsync(resizedMs);
                    break;
                case LSCoreContractsConstants.ImageTypesMIME.Png:
                    await img.SaveAsPngAsync(resizedMs);
                    break;
                default:
                    return LSCoreFileResponse.BadRequest();
            }

            response.Payload = new LSCore.Contracts.Dtos.LSCoreFileDto()
            {
                Data = resizedMs.ToArray(),
                ContentType = imageResponse.Payload.ContentType,
                Tags = imageResponse.Payload.Tags,
            };

            return response;
        }
    }
}
