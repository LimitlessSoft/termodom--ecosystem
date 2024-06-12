// using TD.Web.Common.Contracts.Requests;
// using TD.Web.Common.Contracts.Entities;
// using TD.Web.Common.Contracts.Helpers;
// using TD.Web.Common.Contracts.Dtos;
// using LSCore.Contracts.IManagers;
//
// namespace TD.Web.Common.Repository.Queries
// {
//     public class GetUsersProductPriceQuery : LSCoreBaseQuery<GetUsersProductPricesRequest, UserPricesDto>
//     {
//         public override ILSCoreResponse<UserPricesDto> Execute(ILSCoreDbContext dbContext)
//         {
//             var response = new LSCoreResponse<UserPricesDto>();
//
//             var product = dbContext.AsQueryable<ProductEntity>()
//                 .Include(x => x.Price)
//                 .FirstOrDefault(x => x.Id == Request!.ProductId);
//             if(product == null)
//                 return LSCoreResponse<UserPricesDto>.NotFound();
//
//             var productPriceGroupLevel = dbContext
//                 .AsQueryable<ProductPriceGroupLevelEntity>()
//                 .FirstOrDefault(x =>
//                     x.IsActive &&
//                     x.UserId == Request!.UserId &&
//                     x.ProductPriceGroupId == product.ProductPriceGroupId);
//
//             response.Payload = new UserPricesDto()
//             {
//                 PriceWithoutVAT = PricesHelpers.CalculateProductPriceByLevel(product.Price.Min, product.Price.Max, productPriceGroupLevel?.Level ?? 0),
//                 VAT = product.VAT
//             };
//
//             return response;
//         }
//     }
// }
