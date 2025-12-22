using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using LSCore.SortAndPage.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.Blogs;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Blogs;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers;

[ApiController]
[LSCoreAuth]
[Permissions(Permission.Access)]
public class BlogsController(IBlogManager blogManager) : ControllerBase
{
	[HttpGet]
	[Route("/blogs/{id}")]
	public BlogsGetDto Get([FromRoute] long id) =>
		blogManager.Get(new LSCoreIdRequest { Id = id });

	[HttpGet]
	[Route("/blogs")]
	public LSCoreSortedAndPagedResponse<BlogsGetDto> GetMultiple(
		[FromQuery] BlogsGetMultipleRequest request
	) => blogManager.GetMultiple(request);

	[HttpPut]
	[Route("/blogs")]
	public long Save([FromBody] BlogsSaveRequest request) =>
		blogManager.Save(request);

	[HttpDelete]
	[Route("/blogs/{id}")]
	public IActionResult Delete([FromRoute] long id)
	{
		blogManager.Delete(new LSCoreIdRequest { Id = id });
		return Ok();
	}

	[HttpPut]
	[Route("/blogs/{id}/publish")]
	public IActionResult Publish([FromRoute] long id)
	{
		blogManager.Publish(new LSCoreIdRequest { Id = id });
		return Ok();
	}

	[HttpPut]
	[Route("/blogs/{id}/unpublish")]
	public IActionResult Unpublish([FromRoute] long id)
	{
		blogManager.Unpublish(new LSCoreIdRequest { Id = id });
		return Ok();
	}
}
