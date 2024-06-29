using TD.Web.Admin.Contracts.Requests.ProductPriceGroup;
using TD.Web.Admin.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TD.Web.Admin.Api.Controllers;

[Authorize]
[ApiController]
public class ProductPriceGroupController (IProductPriceGroupManager productPriceGroupManager) : ControllerBase
{
    [HttpPut]
    [Route("/products-prices-groups")]
    public long Save (ProductPriceGroupSaveRequest request) =>
        productPriceGroupManager.Save(request);

    [HttpGet]
    [Route("/products-prices-groups")]
    public List<ProductPriceGroupGetDto> GetMultiple() =>
        productPriceGroupManager.GetMultiple();

    [HttpDelete]
    [Route("/products-prices-groups/{Id}")]
    public void Delete([FromRoute]LSCoreIdRequest request) =>
        productPriceGroupManager.Delete(request);
}