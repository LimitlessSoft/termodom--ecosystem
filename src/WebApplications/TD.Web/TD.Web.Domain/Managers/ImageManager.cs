using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.Interfaces.IManagers;
using TD.Web.Contracts.Requests.Images;

namespace TD.Web.Domain.Managers
{
    public class ImageManager : BaseManager<ImageManager>, IImageManager
    {
        private readonly MinioManager _minioManager;
        public ImageManager(MinioManager minioManager, ILogger<ImageManager> logger)
            : base(logger)
        {
            _minioManager = minioManager;
        }

        public async Task<Response<string>> UploadAsync(ImagesUploadRequest request)
        {
            var response = new Response<string>();

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

                uploadedFileName = Path.Combine(uploadedFileName, extension);

                await _minioManager.UploadAsync(stream, Path.Combine(Contracts.Constants.DefaultImageFolderPath, uploadedFileName), request.Image.ContentType, tags);
            }

            return new Response<string>(uploadedFileName);
        }

        public async Task<FileResponse> GetImageAsync(ImagesGetRequest request)
        {
            var response = new FileResponse();

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
                case Core.Contracts.Constants.ImageTypesMIME.Jpeg:
                    await img.SaveAsJpegAsync(resizedMs);
                    break;
                case Core.Contracts.Constants.ImageTypesMIME.Png:
                    await img.SaveAsPngAsync(resizedMs);
                    break;
                default:
                    return FileResponse.BadRequest();
            }

            response.Tags = new Dictionary<string, string>(imageResponse.Payload.Tags);
            response.Data = resizedMs.ToArray();
            response.ContentType = imageResponse.Payload.ContentType;

            return response;
        }
    }
}
