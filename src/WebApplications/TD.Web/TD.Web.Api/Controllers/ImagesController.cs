using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Web.Contracts.Dtos.Images;
using TD.Web.Contracts.Interfaces.IManagers;
using TD.Web.Contracts.Requests.Images;

namespace TD.Web.Api.Controllers
{
    [ApiController]
    public class ImagesController: ControllerBase
    {
        private readonly IImagesManager _imagesManager;
        private readonly MinioManager _minioManager;

        public ImagesController(IImagesManager imagesManager, MinioManager minioManager)
        {
            _imagesManager = imagesManager;
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
        public Task<Response<ImagesGetDto>> GetImage([FromQuery]ImagesGetRequest request)
        {
            return _imagesManager.GetImageAsync(request);
        }
    }
}
