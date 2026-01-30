using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using LSCore.Exceptions;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.AiContent;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.AiContent;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Admin.Api.Controllers;

[ApiController]
[LSCoreAuth]
[Permissions(Permission.Access)]
public class ProductsController(
	IProductManager productManager,
	IUserManager userManager,
	IAiContentManager aiContentManager)
	: ControllerBase
{
	[HttpGet]
	[Route("/products/{id}")]
	public ProductsGetDto Get([FromRoute] int id) =>
		productManager.Get(new LSCoreIdRequest { Id = id });

	[HttpGet]
	[Route("/products/by-slug/{slug}")]
	public IActionResult GetBySlug([FromRoute] string slug)
	{
		var product = productManager.GetBySlug(slug);
		if (product == null)
			return NotFound();
		return Ok(product);
	}

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

	// === AI VALIDATION ENDPOINTS ===

	[HttpPost]
	[Route("/products/{id}/ai/validate/name")]
	public Task<AiValidationResultDto> ValidateProductName(
		[FromRoute] long id,
		[FromBody] ValidateFieldRequest request)
		=> aiContentManager.ValidateProductNameAsync(id, request);

	[HttpPost]
	[Route("/products/{id}/ai/validate/description")]
	public Task<AiValidationResultDto> ValidateProductDescription(
		[FromRoute] long id,
		[FromBody] ValidateFieldRequest request)
		=> aiContentManager.ValidateProductDescriptionAsync(id, request);

	[HttpPost]
	[Route("/products/{id}/ai/validate/short-description")]
	public Task<AiValidationResultDto> ValidateProductShortDescription(
		[FromRoute] long id,
		[FromBody] ValidateFieldRequest request)
		=> aiContentManager.ValidateProductShortDescriptionAsync(id, request);

	[HttpPost]
	[Route("/products/{id}/ai/validate/meta")]
	public Task<AiValidationResultDto> ValidateProductMeta(
		[FromRoute] long id,
		[FromBody] ValidateMetaRequest request)
		=> aiContentManager.ValidateProductMetaAsync(id, request);

	// === AI GENERATION ENDPOINTS ===

	[HttpPost]
	[Route("/products/{id}/ai/generate/description")]
	public Task<AiGeneratedContentDto> GenerateProductDescription(
		[FromRoute] long id,
		[FromBody] GenerateContentRequest request)
		=> aiContentManager.GenerateProductDescriptionAsync(id, request);

	[HttpPost]
	[Route("/products/{id}/ai/generate/short-description")]
	public Task<AiGeneratedContentDto> GenerateProductShortDescription(
		[FromRoute] long id,
		[FromBody] GenerateContentRequest request)
		=> aiContentManager.GenerateProductShortDescriptionAsync(id, request);

	[HttpPost]
	[Route("/products/{id}/ai/generate/meta")]
	public Task<AiGeneratedContentDto> GenerateProductMeta([FromRoute] long id)
		=> aiContentManager.GenerateProductMetaAsync(id);
}
