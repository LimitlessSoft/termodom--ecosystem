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

        public ImagesController(IImageManager imagesManager, MinioManager minioManager)
        {
            _imagesManager = imagesManager;
            _minioManager = minioManager;
        }

        [HttpPost]
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
