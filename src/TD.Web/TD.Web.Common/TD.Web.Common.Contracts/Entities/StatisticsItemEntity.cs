using TD.Web.Common.Contracts.Enums;
using LSCore.Contracts.Entities;

namespace TD.Web.Common.Contracts.Entities;

public class StatisticsItemEntity : LSCoreEntity
{
    public StatisticType Type { get; set; }
    public string? Value { get; set; }
}