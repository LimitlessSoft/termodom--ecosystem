using Omu.ValueInjecter;
using TD.Core.Contracts;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Repository.Mappings
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

            if(request.IsNew)
                entity.Price = new ProductPriceEntity();
        }
    }
}
