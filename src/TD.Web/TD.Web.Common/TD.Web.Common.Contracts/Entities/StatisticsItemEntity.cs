using LSCore.Repository.Contracts;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Entities;

public class StatisticsItemEntity : LSCoreEntity
{
	public StatisticType Type { get; set; }
	public string? Value { get; set; }
}
