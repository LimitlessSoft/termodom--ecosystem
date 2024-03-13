using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using LSCore.Framework;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ProductPriceGroup;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    public class ProductPriceGroupController : ControllerBase
    {
        private readonly IProductPriceGroupManager _productsPriceGroupManager;
        public ProductPriceGroupController(IProductPriceGroupManager productPriceGroupManager)
        {
            _productsPriceGroupManager = productPriceGroupManager;
        }

        [HttpPut]
        [Route("/products-prices-groups")]
        public LSCoreResponse<long> Save (ProductPriceGroupSaveRequest request) =>
            _productsPriceGroupManager.Save(request);

        [HttpGet]
        [Route("/products-prices-groups")]
        public LSCoreListResponse<ProductPriceGroupGetDto> GetMultiple() =>
            _productsPriceGroupManager.GetMultiple();

        [HttpDelete]
        [Route("/products-prices-groups/{Id}")]
        public LSCoreResponse Delete([FromRoute]LSCoreIdRequest request) =>
            _productsPriceGroupManager.Delete(request);
    }
}
