using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using LSCore.Exceptions;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Admin.Api.Controllers;

[ApiController]
[LSCoreAuth]
[Permissions(Permission.Access)]
public class ProductsController(IProductManager productManager, IUserManager userManager)
	: ControllerBase
{
	[HttpGet]
	[Route("/products/{id}")]
	public ProductsGetDto Get([FromRoute] int id) =>
		productManager.Get(new LSCoreIdRequest { Id = id });

	[HttpPost]
	[Route("/products/{Id}/search-keywords")]
	public IActionResult AppendSearchKeywords(
		[FromRoute] LSCoreIdRequest idRequest,
		[FromBody] CreateProductSearchKeywordRequest request
	)
	{
		request.Id = idRequest.Id;
		productManager.AppendSearchKeywords(request);
		return Ok();
	}

	[HttpDelete]
	[Route("/products/{Id}/search-keywords")]
	public IActionResult DeleteSearchKeywords(
		[FromRoute] LSCoreIdRequest idRequest,
		[FromBody] DeleteProductSearchKeywordRequest request
	)
	{
		request.Id = idRequest.Id;
		productManager.DeleteSearchKeywords(request);
		return Ok();
	}

    [HttpGet]
    [Route("/products/{Id}/linked")]
    public IActionResult GetLinked(
        [FromRoute] LSCoreIdRequest idRequest) {
        return Ok(productManager.GetLinked(idRequest));
    }

    [HttpPost]
    [Route("/products/{Id}/linked/{Link}")]
    public IActionResult CreateLinked(
        [FromRoute] LSCoreIdRequest idRequest,
        [FromRoute] string Link) {
        productManager.SetLink(idRequest, Link);
        return Ok();
    }

    [HttpDelete]
    [Route("/products/{Id}/linked")]
    public IActionResult DeleteLinked(
        [FromRoute] LSCoreIdRequest idRequest) {
        productManager.SetLink(idRequest, string.Empty);
        return Ok();
    }


	[HttpGet]
	[Route("/products")]
	public List<ProductsGetDto> GetMultiple([FromQuery] ProductsGetMultipleRequest request) =>
		productManager.GetMultiple(request);

	[HttpPut]
	[Route("/products")]
	public long Save([FromBody] ProductsSaveRequest request)
	{
		if (
			!userManager.HasPermission(Permission.Admin_Products_EditAll)
			&& request.Id.HasValue == false
			&& !productManager.HasPermissionToEdit(request.Id!.Value)
		)
			throw new LSCoreForbiddenException();

		return productManager.Save(request);
	}

	[HttpPut]
	[Route("/products-update-max-web-osnove")]
	public void UpdateMaxWebOsnove([FromBody] ProductsUpdateMaxWebOsnoveRequest request) =>
		productManager.UpdateMaxWebOsnove(request);

	[HttpPut]
	[Route("/products-update-min-web-osnove")]
	public void UpdateMinWebOsnove([FromBody] ProductsUpdateMinWebOsnoveRequest request) =>
		productManager.UpdateMinWebOsnove(request);

	[HttpGet]
	[Route("/products-search")]
	public List<ProductsGetDto> GetSearch([FromQuery] ProductsGetSearchRequest request) =>
		productManager.GetSearch(request);

	[HttpGet]
	[Route("/products-classifications")]
	public List<IdNamePairDto> GetClassifications() => productManager.GetClassifications();
}
