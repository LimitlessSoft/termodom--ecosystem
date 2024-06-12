using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Admin.Contracts.Dtos.Products;
using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using LSCore.Contracts.Dtos;
using TD.Web.Admin.Contracts.Interfaces.IManagers;

namespace TD.Web.Admin.Api.Controllers;

[ApiController]
public class ProductsController (IProductManager productManager) : ControllerBase
{
    [HttpGet]
    [Route("/products/{id}")]
    public ProductsGetDto Get([FromRoute] int id) =>
        productManager.Get(new LSCoreIdRequest() { Id = id });

    [HttpGet]
    [Route("/products")]
    public List<ProductsGetDto> GetMultiple([FromQuery] ProductsGetMultipleRequest request) =>
        productManager.GetMultiple(request);

    [HttpPut]
    [Route("/products")]
    public long Save([FromBody] ProductsSaveRequest request) =>
        productManager.Save(request);

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