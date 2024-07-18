using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Images;
using TD.Web.Common.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace TD.Web.Admin.Api.Controllers;

[ApiController]
public class ImagesController(IImageManager imagesManager) : ControllerBase
{
    [HttpPost]
    [Route("/images")]
    public Task<string> Upload([FromForm]ImagesUploadRequest request) =>
        imagesManager.UploadAsync(request);

    [HttpGet]
    [Route("/images")]
    public async Task<FileDto> GetImage([FromQuery]ImagesGetRequest request) =>
        await imagesManager.GetImageAsync(request);
}