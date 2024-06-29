using TD.Web.Public.Contracts.Requests.ProductsGroups;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Dtos.ProductsGroups;
using Microsoft.AspNetCore.Mvc;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
public class ProductsGroupsController (IProductGroupManager productGroupManager) : ControllerBase
{
    [HttpGet]
    [Route("/products-groups/{name}")]
    public ProductsGroupsGetDto Get([FromRoute] string name) =>
        productGroupManager.Get(name);

    [HttpGet]
    [Route("/products-groups")]
    public List<ProductsGroupsGetDto> GetMultiple([FromQuery] ProductsGroupsGetRequest request) =>
        productGroupManager.GetMultiple(request);
}