using Microsoft.AspNetCore.Mvc;
using System.Text;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Managers;
using TD.Web.Contracts.Dtos.Products;
using TD.Web.Contracts.Interfaces.Managers;
using TD.Web.Contracts.Requests.Products;

namespace TD.Web.Api.Controllers
{
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductManager _productManager;
        private readonly MinioManager _minioManager;

        public ProductsController(IProductManager productManager, MinioManager minioManager)
        {
            _productManager = productManager;
            _minioManager = minioManager;
        }

        [HttpGet]
        [Route("/products/ping")]
        public async Task<Response> assafAsync()
        {
            var ms = new MemoryStream();
            ms.Write(Encoding.UTF8.GetBytes("Hellooooo"));
            ms.Seek(0, SeekOrigin.Begin);
            await _minioManager.Upload(ms, "someFile.txt", "text/plain");
            return new Response();
        }

        [HttpGet]
        [Route("/products/{id}")]
        public Response<ProductsGetDto> Get([FromRoute] int id)
        {
            return _productManager.Get(new IdRequest(id));
        }

        [HttpGet]
        [Route("/products")]
        public ListResponse<ProductsGetDto> GetMultiple([FromQuery] ProductsGetMultipleRequest request)
        {
            return _productManager.GetMultiple(request);
        }

        [HttpPut]
        [Route("/products")]
        public Response<long> Save([FromBody] ProductsSaveRequest request)
        {
            return _productManager.Save(request);
        }

        [HttpGet]
        [Route("/products-search")]
        public ListResponse<ProductsGetDto> GetSearch([FromQuery] ProductsGetSearchRequest request)
        {
            return _productManager.GetSearch(request);
        }
    }
}
