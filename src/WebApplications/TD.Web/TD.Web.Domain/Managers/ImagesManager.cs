using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

        public async Task<Response<string>> Upload(ImagesUploadRequest request)
        {
            var response = new Response<string>();
            if (request.IsRequestInvalid(response))
                return response;

            var ms = new MemoryStream();
            await request.Image.CopyToAsync(ms);
            Stream k = ms;
            await _minioManager.UploadAsync(k, request.Image.FileName, request.Image.ContentType);

            return new Response<string>(request.Image.FileName);
        }
    }
}
