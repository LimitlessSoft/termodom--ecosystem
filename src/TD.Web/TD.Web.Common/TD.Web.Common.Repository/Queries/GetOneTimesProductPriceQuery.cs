using LSCore.Contracts.Http;
using LSCore.Contracts.Http.Interfaces;
using LSCore.Contracts.IManagers;
using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Common.Contracts.Requests;

namespace TD.Web.Common.Repository.Queries
{
    public class GetOneTimesProductPriceQuery : LSCoreBaseQuery<GetOneTimesProductPricesRequest, OneTimePricesDto>
    {
        public override ILSCoreResponse<OneTimePricesDto> Execute(ILSCoreDbContext dbContext)
        {
            var response = new LSCoreResponse<OneTimePricesDto>();

            var product = dbContext.AsQueryable<ProductEntity>()
                .Include(x => x.Price)
                .FirstOrDefault(x => x.Id == Request!.ProductId);
            if (product == null)
                return LSCoreResponse<OneTimePricesDto>.NotFound();

            var priceK = PricesHelpers.CalculatePriceK(product.Price.Min, product.Price.Max);
            response.Payload = new OneTimePricesDto()
            {
                MinPrice = product.Price.Max - (priceK / Constants.NumberOfCartValueStages * PricesHelpers.CalculateCartLevel(Constants.MaximumCartValueForDiscount)),
                MaxPrice = product.Price.Max,
            };

            return response;
        }
    }
}
