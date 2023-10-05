using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using TD.Core.Contracts.Dtos;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.Interfaces.IManagers;
using TD.Web.Contracts.Requests.Images;

namespace TD.Web.Domain.Managers
{
    public class ImagesManager : BaseManager<ImagesManager>, IImagesManager
    {
        private readonly string _defaultImagePath = "/images/";
        private readonly MinioManager _minioManager;
        public ImagesManager(MinioManager minioManager,ILogger<ImagesManager> logger) : base(logger)
        {
            _minioManager = minioManager;
        }

        public async Task<Response<string>> UploadAsync(ImagesUploadRequest request)
        {
            var response = new Response<string>();
            var hash = "";
            var ext = Path.GetExtension(request.Image.FileName);
            if (request.IsRequestInvalid(response))
                return response;

            using (Stream stream = request.Image.OpenReadStream()) 
            {
                var hashCreator = SHA256.Create();
                Dictionary<string, string> tags = null;
                if (request.AltText != null)
                {
                    tags = new Dictionary<string, string>();
                    tags["alt"] = request.AltText;
                }
                
                var name = hashCreator.ComputeHash(Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss.fff")));
                foreach (byte b in name)
                {
                    hash += $"{b:X2}";
                }
                await _minioManager.UploadAsync(stream, _defaultImagePath + hash + ext, request.Image.ContentType, tags);
            }

            return new Response<string>(hash + ext);
        }

        public async Task<Response<FileDto>> GetImageAsync(ImagesGetRequest request)
        {
            var image = await _minioManager.DownloadAsync(request.Image);
            //image.Payload.Data.
            return image;
        }
    }
}
