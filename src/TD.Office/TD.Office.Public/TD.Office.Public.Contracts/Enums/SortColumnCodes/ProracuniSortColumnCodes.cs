using System.Linq.Expressions;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Enums.SortColumnCodes;

public static class ProracuniSortColumnCodes
{
    public enum Proracuni
    {
        Id,
        CreatedAt
    }

    public static Dictionary<
        Proracuni,
        Expression<Func<ProracunEntity, object>>
    > ProracuniSortRules =
        new() { { Proracuni.Id, x => x.Id }, { Proracuni.CreatedAt, x => x.CreatedAt } };
}
