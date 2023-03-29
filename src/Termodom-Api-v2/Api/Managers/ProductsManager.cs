using Api.Dtos;
using Api.Interfaces.Managers;
using Infrastructure.Entities.ApiV2;
using Infrastructure.Framework;
using Omu.ValueInjecter;

namespace Api.Managers
{
    public class ProductsManager : IProductsManager
    {
        private readonly ApiDbContext _dbContext;

        public ProductsManager(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public APIResponse<IQueryable<PublicProductDto>> Queriable()
        {
            var products = _dbContext.Products.Select(x => (PublicProductDto)(new PublicProductDto()).InjectFrom(x)).AsQueryable();
            return new APIResponse<IQueryable<PublicProductDto>>(products);
        }
    }
}
