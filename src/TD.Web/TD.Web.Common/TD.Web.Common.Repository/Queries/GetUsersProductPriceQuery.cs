using LSCore.Contracts.Http;
using LSCore.Contracts.Http.Interfaces;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Requests;
using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Common.Contracts.Requests;

namespace TD.Web.Common.Repository.Queries
{
    public class GetUsersProductPriceQuery : LSCoreBaseQuery<GetUsersProductPricesRequest, UserPricesDto>
    {
        public override ILSCoreResponse<UserPricesDto> Execute(ILSCoreDbContext dbContext)
        {
            var response = new LSCoreResponse<UserPricesDto>();

            var product = dbContext.AsQueryable<ProductEntity>()
                .Include(x => x.Price)
                .FirstOrDefault(x => x.Id == Request!.ProductId);
            if(product == null)
                return LSCoreResponse<UserPricesDto>.NotFound();

            var productPriceGroupLevel = dbContext
                .AsQueryable<ProductPriceGroupLevelEntity>()
                .FirstOrDefault(x =>
                    x.IsActive &&
                    x.UserId == Request!.UserId &&
                    x.ProductPriceGroupId == product.ProductPriceGroupId);

            response.Payload = new UserPricesDto()
            {
                PriceWithoutVAT = product.Price.Max - ((PricesHelpers.CalculatePriceK(product.Price.Min, product.Price.Max) / (Constants.NumberOfProductPriceGroupLevels - 1)) * productPriceGroupLevel?.Level ?? 0),
                VAT = product.VAT
            };

            return response;
        }
    }
}
