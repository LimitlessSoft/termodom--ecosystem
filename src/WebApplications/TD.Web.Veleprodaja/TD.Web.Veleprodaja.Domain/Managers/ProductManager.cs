using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;
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

        public ListResponse<ProductsGetDto> GetMultiple(ProductsGetRequest request) => new ListResponse<ProductsGetDto>(
            Queryable()
            .Where(x => 
                (!request.Id.HasValue || x.Id == request.Id.Value))
            .ToList()
            .ToProductsGetDtoList());

        public Response<ProductsGetDto> Put(ProductsPutRequest request)
        {
            var response = new Response<ProductsGetDto>();
            if (request.IsRequestInvalid(response))
                return response;

            return new Response<ProductsGetDto>(Save(request).ToProductsGetDto());
        }
    }
}
