// using LSCore.Contracts.Interfaces;
// using Omu.ValueInjecter;
// using TD.Web.Admin.Contracts.Requests.Products;
// using TD.Web.Common.Contracts.Entities;
// using TD.Web.Common.Repository;
//
// namespace TD.Web.Admin.Repository.Mappings
// {
//     public class ProductsSaveRequestMapping : ILSCoreMap<ProductEntity, ProductsSaveRequest>
//     {
//         private readonly WebDbContext _webDbContext;
//         public ProductsSaveRequestMapping(WebDbContext dbContext)
//         {
//             _webDbContext = dbContext;
//         }
//
//         public void Map(ProductEntity entity, ProductsSaveRequest request)
//         {
//             entity.InjectFrom(request);
//             entity.CatalogId = entity.CatalogId!.ToUpper();
//
//             if(request.IsNew)
//                 entity.Price = new ProductPriceEntity();
//         }
//     }
// }
