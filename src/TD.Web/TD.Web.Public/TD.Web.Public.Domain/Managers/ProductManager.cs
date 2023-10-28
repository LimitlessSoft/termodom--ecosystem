using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Extensions;
using TD.Core.Domain.Managers;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;
using TD.Web.Public.Contrats.Dtos.Products;
using TD.Web.Public.Contrats.Interfaces.IManagers;
using TD.Web.Public.Contrats.Requests.Products;

namespace TD.Web.Public.Domain.Managers
{
    public class ProductManager : BaseManager<ProductManager, ProductEntity>, IProductManager
    {
        public ProductManager(ILogger<ProductManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public ListResponse<ProductsGetDto> GetMultiple(ProductsGetRequest request) =>
            new ListResponse<ProductsGetDto>(
                Queryable(x => x.IsActive)
                .ToDtoList<ProductsGetDto, ProductEntity>());
    }
}
