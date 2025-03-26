using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.Entities;

public class UslovFormiranjaWebCeneEntity : LSCoreEntity
{
	public long WebProductId { get; set; }
	public UslovFormiranjaWebCeneType Type { get; set; }
	public decimal Modifikator { get; set; }
}
