using TD.Web.Admin.Contracts.Requests.ProductPriceGroup;
using TD.Web.Admin.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using LSCore.Contracts.Requests;
using LSCore.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers;

[LSCoreAuthorize]
[ApiController]
[Permissions(Permission.Access)]
public class ProductPriceGroupController (IProductPriceGroupManager productPriceGroupManager) : ControllerBase
{
    [HttpGet]
    [Route("/products-prices-groups")]
    public List<ProductPriceGroupGetDto> GetMultiple() =>
        productPriceGroupManager.GetMultiple();

    [HttpDelete]
    [Route("/products-prices-groups/{Id}")]
    public void Delete([FromRoute]LSCoreIdRequest request) =>
        productPriceGroupManager.Delete(request);
    
    [HttpPut]
    [Route("/products-prices-groups")]
    public long Save (ProductPriceGroupSaveRequest request) =>
        productPriceGroupManager.Save(request);
}