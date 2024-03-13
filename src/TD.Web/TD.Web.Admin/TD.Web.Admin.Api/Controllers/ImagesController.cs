using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Requests.Images;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    public class ImagesController: ControllerBase
    {
        private readonly IImageManager _imagesManager;

        public ImagesController(IImageManager imagesManager)
        {
            _imagesManager = imagesManager;
        }

        [HttpPost]
        [Route("/images")]
        public Task<LSCoreResponse<string>> Upload([FromForm]ImagesUploadRequest request) =>
            _imagesManager.UploadAsync(request);

        [HttpGet]
        [Route("/images")]
        public async Task<LSCoreFileResponse> GetImage([FromQuery]ImagesGetRequest request) =>
            await _imagesManager.GetImageAsync(request);
    }
}
