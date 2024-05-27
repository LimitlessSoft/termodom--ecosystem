using System.Linq.Expressions;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Enums.SortColumnCodes
{
    public static class UsersSortColumnCodes
    {
        public enum Users
        {
            Id,
            Username,
            Nickname,
        }
        

        public static Dictionary<Users, Expression<Func<UserEntity, object>>> UsersSortRules = new()
        {
            { Users.Id, x => x.Id },
            { Users.Username, x => x.Username },
            { Users.Nickname, x => x.Nickname }
        };
    }
}