using TD.Web.Admin.Contracts.Requests.ProductsGroups;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Dtos.ProductsGroups;
using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TD.Web.Admin.Api.Controllers;

[Authorize]
[ApiController]
public class ProductsGroupsController (IProductGroupManager productGroupManager) : ControllerBase
{
    [HttpGet]
    [Route("/products-groups")]
    public List<ProductsGroupsGetDto> GetMultiple() =>
        productGroupManager.GetMultiple();

    [HttpGet]
    [Route("/products-groups/{id}")]
    public ProductsGroupsGetDto Get([FromRoute]int id) =>
        productGroupManager.Get(new LSCoreIdRequest() { Id = id });

    [HttpPut]
    [Route("/products-groups")]
    public long Save([FromBody]ProductsGroupsSaveRequest request) =>
        productGroupManager.Save(request);

    [HttpDelete]
    [Route("/products-groups/{Id}")]
    public void Delete([FromRoute]ProductsGroupsDeleteRequest request) =>
        productGroupManager.Delete(request);

    [HttpPut]
    [Route("/products-groups/{Id}/type")]
    public void UpdateType([FromRoute] LSCoreIdRequest idRequest, [FromBody] ProductsGroupUpdateTypeRequest request)
    {
        request.Id = idRequest.Id;
        productGroupManager.UpdateType(request);
    }
}