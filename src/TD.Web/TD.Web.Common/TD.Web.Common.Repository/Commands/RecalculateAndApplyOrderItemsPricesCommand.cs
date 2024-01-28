using LSCore.Contracts.Http;
using LSCore.Contracts.Http.Interfaces;
using LSCore.Contracts.IManagers;
using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Common.Contracts.Requests.OrderItems;

namespace TD.Web.Common.Repository.Commands
{
    public class RecalculateAndApplyOrderItemsPricesCommand : LSCoreBaseCommand<RecalculateAndApplyOrderItemsPricesCommandRequest>
    {
        public override ILSCoreResponse Execute(ILSCoreDbContext dbContext)
        {
            if(Request == null)
                throw new NullReferenceException(nameof(Request));

            var orderItems = dbContext.AsQueryable<OrderItemEntity>()
                .Where(x =>
                    x.IsActive &&
                    x.OrderId == Request!.Id)
                .Include(x => x.Product)
                .ThenInclude(x => x.Price);

            if(Request.UserId == null)
                CalculateAndApplyOneTimePrices();
            else
                CalculateAndApplyUserPrices();

            dbContext.UpdateMultiple(orderItems);
            return new LSCoreResponse();

            #region Inner methods
            void CalculateAndApplyOneTimePrices()
            {
                var totalCartValueWithoutDiscount = orderItems.Sum(x => x.Product.Price.Max * x.Quantity);

                foreach (var item in orderItems)
                {
                    item.PriceWithoutDiscount = item.Product.Price.Max;
                    item.Price = PricesHelpers.CalculateOneTimeCartPrice(item.Product.Price.Min, item.Product.Price.Max, totalCartValueWithoutDiscount);
                }
            }

            void CalculateAndApplyUserPrices()
            {
                var user = dbContext.AsQueryable<UserEntity>()
                    .Include(x => x.ProductPriceGroupLevels)
                    .FirstOrDefault(x => x.Id == Request.UserId);

                foreach (var item in orderItems)
                {
                    item.PriceWithoutDiscount = item.Product.Price.Max;
                    item.Price = PricesHelpers.CalculateProductPriceByLevel(
                        item.Product.Price.Min,
                        item.Product.Price.Max,
                        user?.ProductPriceGroupLevels.First(z => z.ProductPriceGroupId == item.Product.ProductPriceGroupId)?.Level ?? 0);
                }
            }
            #endregion
        }
    }
}
