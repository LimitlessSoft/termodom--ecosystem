using LSCore.SortAndPage.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Public.Contracts.Dtos.Blogs;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Blogs;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
public class BlogsController(IBlogManager blogManager) : ControllerBase
{
	[HttpGet]
	[Route("/blogs")]
	public async Task<LSCoreSortedAndPagedResponse<BlogGetDto>> GetMultiple(
		[FromQuery] GetMultipleBlogsRequest request
	) => await blogManager.GetMultipleAsync(request);

	[HttpGet]
	[Route("/blogs/{slug}")]
	public async Task<BlogGetSingleDto> GetSingle([FromRoute] string slug) =>
		await blogManager.GetSingleAsync(new GetSingleBlogRequest { Slug = slug });
}
