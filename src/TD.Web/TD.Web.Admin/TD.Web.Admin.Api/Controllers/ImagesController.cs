using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Images;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    public class ImagesController: ControllerBase
    {
        private readonly IImageManager _imagesManager;
        private readonly LSCoreMinioManager _minioManager;

        public ImagesController(IImageManager imagesManager, LSCoreMinioManager minioManager)
        {
            _imagesManager = imagesManager;
            _minioManager = minioManager;
        }

        [HttpPost]
        [Route("/images")]
        public Task<LSCoreResponse<string>> Upload([FromForm]ImagesUploadRequest request)
        {
            return _imagesManager.UploadAsync(request);
        }

        [HttpGet]
        [Route("/images")]
        public async Task<LSCoreFileResponse> GetImage([FromQuery]ImagesGetRequest request)
        {
            return await _imagesManager.GetImageAsync(request);
        }
    }
}
