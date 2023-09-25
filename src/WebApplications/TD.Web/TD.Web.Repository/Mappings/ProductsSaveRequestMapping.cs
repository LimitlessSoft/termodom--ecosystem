using Omu.ValueInjecter;
using TD.Core.Contracts;
using TD.Web.Contracts.Entities;
using TD.Web.Contracts.Requests.Products;

namespace TD.Web.Repository.Mappings
{
    public class ProductsSaveRequestMapping : IMap<ProductEntity, ProductsSaveRequest>
    {
        private readonly WebDbContext _webDbContext;
        public ProductsSaveRequestMapping(WebDbContext dbContext)
        {
            _webDbContext = dbContext;
        }

        public void Map(ProductEntity entity, ProductsSaveRequest request)
        {
            entity.InjectFrom(request);
            entity.Price = new ProductPriceEntity();
        }
    }
}
