using static TD.Web.Common.Contracts.Enums.SortColumnCodes.StoresSortColumnCodes;
using System.Linq.Expressions;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Enums.SortColumnCodes
{
    public static class CitiesSortColumnCodes
    {
        public enum Cities
        {
            Name
        }

        public static Dictionary<Cities, Expression<Func<CityEntity, object>>> CitiesSortRules = new()
        {
            { Cities.Name, x => x.Name }
        };
    }
}
