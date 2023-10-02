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
    public class ImagesManager : BaseManager<ImagesManager>, IImagesManager
    {
        private readonly MinioManager _minioManager;
        public ImagesManager(MinioManager minioManager,ILogger<ImagesManager> logger) : base(logger)
        {
            _minioManager = minioManager;
        }

        public async Task<Response<string>> UploadAsync(ImagesUploadRequest request)
        {
            var response = new Response<string>();
            if (request.IsRequestInvalid(response))
                return response;

            using (Stream stream = request.Image.OpenReadStream()) 
            {
                SHA256 hashCreator = SHA256.Create();
                Dictionary<string, string> tags = new Dictionary<string, string>();
                tags["alt"] = request.AltText ?? null;  
                var name = hashCreator.ComputeHash(Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString()));
                await _minioManager.UploadAsync(stream, name.ToString(), request.Image.ContentType, tags);
            }

            return new Response<string>(request.Image.FileName);
        }
    }
}
