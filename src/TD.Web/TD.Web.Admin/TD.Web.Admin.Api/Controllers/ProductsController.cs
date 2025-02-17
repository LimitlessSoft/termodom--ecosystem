using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Common.Contracts.Attributes;
using Microsoft.AspNetCore.Authorization;
using TD.Web.Common.Contracts.Enums;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using LSCore.Contracts.Dtos;
using LSCore.Framework.Attributes;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Admin.Api.Controllers;

[ApiController]
[LSCoreAuthorize]
[Permissions(Permission.Access)]
public class ProductsController (IProductManager productManager, IUserManager userManager) : ControllerBase
{
    [HttpGet]
    [Route("/products/{id}")]
    public ProductsGetDto Get([FromRoute] int id) =>
        productManager.Get(new LSCoreIdRequest() { Id = id });

    [HttpPost]
    [Route("/products/{Id}/search-keywords")]
    public IActionResult AppendSearchKeywords([FromRoute] LSCoreIdRequest idRequest,
        [FromBody] CreateProductSearchKeywordRequest request)
    {
        request.Id = idRequest.Id;
        productManager.AppendSearchKeywords(request);
        return Ok();
    }
    
    [HttpDelete]
    [Route("/products/{Id}/search-keywords")]
    public IActionResult DeleteSearchKeywords([FromRoute] LSCoreIdRequest idRequest, [FromBody] DeleteProductSearchKeywordRequest request)
    {
        request.Id = idRequest.Id;
        productManager.DeleteSearchKeywords(request);
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
        if (!userManager.HasPermission(Permission.Admin_Products_EditAll) && !request.IsNew && !productManager.HasPermissionToEdit(request.Id!.Value))
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
    public List<LSCoreIdNamePairDto> GetClassifications() =>
        productManager.GetClassifications();
}