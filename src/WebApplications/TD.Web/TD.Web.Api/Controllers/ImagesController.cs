using Microsoft.AspNetCore.Mvc;
using System.Text;
using TD.Core.Contracts.Dtos;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Web.Contracts.Interfaces.IManagers;
using TD.Web.Contracts.Requests.Images;

namespace TD.Web.Api.Controllers
{
    [ApiController]
    public class ImagesController: ControllerBase
    {
        private readonly IImageManager _imagesManager;
        private readonly MinioManager _minioManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImagesController(IImageManager imagesManager, MinioManager minioManager, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            _imagesManager = imagesManager;
            _imagesManager.SetContextInfo(_httpContextAccessor.HttpContext);

            _minioManager = minioManager;
        }

        [HttpPost]
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [Route("/images")]
        public Task<Response<string>> Upload([FromForm]ImagesUploadRequest request)
        {
            return _imagesManager.UploadAsync(request);
        }

        [HttpGet]
        [Route("/images")]
        public async Task<FileResponse> GetImage([FromQuery]ImagesGetRequest request)
        {
            return await _imagesManager.GetImageAsync(request);
        }
    }
}
