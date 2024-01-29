using System.Linq.Expressions;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.Enums.SortColumnCodes
{
    public static class OrdersSortColumnCodes
    {
        public enum Orders
        {
            Date
        }

        public static Dictionary<Orders, Expression<Func<OrderEntity, object>>> OrdersSortRules = new()
        {
            { Orders.Date, x => x.CreatedAt }
        };
    }
}
