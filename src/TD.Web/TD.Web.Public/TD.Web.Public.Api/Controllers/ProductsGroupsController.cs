using Microsoft.AspNetCore.Mvc;
using TD.Web.Public.Contracts.Dtos.ProductsGroups;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.ProductsGroups;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
public class ProductsGroupsController(IProductGroupManager productGroupManager) : ControllerBase
{
	[HttpGet]
	[Route("/products-groups/{src}")]
	public ProductsGroupsGetDto Get([FromRoute] string src) => productGroupManager.Get(src);

	[HttpGet]
	[Route("/products-groups")]
	public List<ProductsGroupsGetDto> GetMultiple([FromQuery] ProductsGroupsGetRequest request) =>
		productGroupManager.GetMultiple(request);
}
