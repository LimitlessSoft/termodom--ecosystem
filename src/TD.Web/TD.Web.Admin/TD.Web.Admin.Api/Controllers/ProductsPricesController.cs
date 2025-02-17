using TD.Web.Admin.Contracts.Requests.ProductsPrices;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Dtos.ProductPrices;
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
public class ProductsPricesController (IProductPriceManager productPriceManager) : ControllerBase
{
    [HttpGet]
    // [Authorize("TestPolicy")]
    [Route("/products-prices")]
    public List<ProductsPricesGetDto> GetMultiple() =>
        productPriceManager.GetMultiple();

    [HttpPut]
    [Route("/products-prices")]
    public long Save(SaveProductPriceRequest request) =>
        productPriceManager.Save(request);

    [HttpDelete]
    [Route("/products-prices/{id}")]
    public void Delete([FromRoute]LSCoreIdRequest request) =>
        productPriceManager.Delete(request);
}