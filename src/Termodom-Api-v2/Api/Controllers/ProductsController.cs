using Api.Dtos;
using Api.Interfaces.Managers;
using Infrastructure.Entities.ApiV2;
using Infrastructure.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsManager _productsManager;

        public ProductsController(IProductsManager productsManager)
        {
            _productsManager = productsManager;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/products")]
        public APIResponse<IQueryable<PublicProductDto>> List()
        {
            return _productsManager.Queriable();
        }
    }
}
