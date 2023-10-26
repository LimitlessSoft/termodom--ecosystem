using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Web.Admin.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ProductPriceGroup;

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
        public Response<long> Save (ProductPriceGroupSaveRequest request)
        {
            return _productsPriceGroupManager.Save(request);
        }

        [HttpGet]
        [Route("/products-prices-groups")]
        public ListResponse<ProductPriceGroupGetDto> GetMultiple()
        {
            return _productsPriceGroupManager.GetMultiple();
        }

        [HttpDelete]
        [Route("/products-prices-groups/{Id}")]
        public Response Delete([FromRoute]IdRequest request)
        {
            return _productsPriceGroupManager.Delete(request);
        }
    }
}
