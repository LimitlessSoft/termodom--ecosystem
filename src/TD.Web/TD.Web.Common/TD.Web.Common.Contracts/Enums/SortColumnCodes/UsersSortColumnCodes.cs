using System.Linq.Expressions;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Enums.SortColumnCodes
{
    public static class UsersSortColumnCodes
    {
        public enum Users
        {
            Id,
            Username
        }

        public static Dictionary<Users, Expression<Func<UserEntity, object>>> UsersSortRules = new()
        {
            { Users.Id, x => x.Id },
            { Users.Username, x => x.Username }
        };
    }
}
