// using TD.Web.Common.Contracts.Requests;
// using TD.Web.Common.Contracts.Helpers;
// using TD.Web.Common.Contracts.Dtos;
// using LSCore.Contracts.IManagers;
// using TD.Web.Common.Contracts;
//
// namespace TD.Web.Common.Repository.Queries
// {
//     public class GetOneTimesProductPriceQuery : LSCoreBaseQuery<GetOneTimesProductPricesRequest, OneTimePricesDto>
//     {
//         public override ILSCoreResponse<OneTimePricesDto> Execute(ILSCoreDbContext dbContext)
//         {
//             var response = new LSCoreResponse<OneTimePricesDto>();
//             
//             if (Request.Product == null)
//                 return LSCoreResponse<OneTimePricesDto>.NotFound();
//
//             var priceK = PricesHelpers.CalculatePriceK(Request.Product.Price.Min, Request.Product.Price.Max);
//             response.Payload = new OneTimePricesDto()
//             {
//                 MinPrice = Request.Product.Price.Max - (priceK / Constants.NumberOfCartValueStages * PricesHelpers.CalculateCartLevel(Constants.MaximumCartValueForDiscount)),
//                 MaxPrice = Request.Product.Price.Max,
//             };
//
//             return response;
//         }
//     }
// }
