using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Images;

namespace TD.Web.Admin.Api.Controllers;

[ApiController]
[Permissions(Permission.Access)]
public class ImagesController(IImageManager imagesManager) : ControllerBase
{
	[HttpPost]
	[Route("/images")]
	public Task<string> Upload([FromForm] ImagesUploadRequest request) =>
		imagesManager.UploadAsync(request);

	[HttpGet]
	[Route("/images")]
	public async Task<FileDto> GetImage([FromQuery] ImagesGetRequest request) =>
		await imagesManager.GetImageAsync(request);
}
