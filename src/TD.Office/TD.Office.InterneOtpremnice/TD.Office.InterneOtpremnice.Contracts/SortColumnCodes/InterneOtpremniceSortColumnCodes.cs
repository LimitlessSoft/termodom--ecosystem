using System.Linq.Expressions;
using TD.Office.InterneOtpremnice.Contracts.Entities;

namespace TD.Office.InterneOtpremnice.Contracts.SortColumnCodes;

public static class InterneOtpremniceSortColumnCodes
{
    public enum InterneOtpremnice
    {
        Id
    }

    public static Dictionary<
        InterneOtpremnice,
        Expression<Func<InternaOtpremnicaEntity, object>>
    > Rules => new() { { InterneOtpremnice.Id, x => x.Id } };
}
