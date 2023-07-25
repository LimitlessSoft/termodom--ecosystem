using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Web.Veleprodaja.Contracts.DtoMappings;
using TD.Web.Veleprodaja.Contracts.Dtos.Products;
using TD.Web.Veleprodaja.Contracts.Entities;
using TD.Web.Veleprodaja.Contracts.IManagers;
using TD.Web.Veleprodaja.Contracts.Requests.Products;
using TD.Web.Veleprodaja.Repository;

namespace TD.Web.Veleprodaja.Domain.Managers
{
    public class ProductManager : BaseManager<ProductManager, Product>, IProductManager
    {
        public ProductManager(ILogger<ProductManager> logger, VeleprodajaDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public ListResponse<ProductsGetDto> GetMultiple() => new ListResponse<ProductsGetDto>(
                Queryable()
                .ToList()
                .ToProductsGetDtoList());

        public Response<ProductsGetDto> Put(ProductsPutRequest request) => new Response<ProductsGetDto>(Save(request).ToProductsGetDto());
    }
}
