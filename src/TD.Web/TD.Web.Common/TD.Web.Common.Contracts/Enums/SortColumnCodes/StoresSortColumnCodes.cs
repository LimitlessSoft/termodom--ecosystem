using System.Linq.Expressions;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Enums.SortColumnCodes
{
    public static class StoresSortColumnCodes
    {
        public enum Stores
        {
            Name
        }

        public static Dictionary<Stores, Expression<Func<StoreEntity, object>>> StoresSortRules = new()
        {
            { Stores.Name, x => x.Name }
        };
    }
}
