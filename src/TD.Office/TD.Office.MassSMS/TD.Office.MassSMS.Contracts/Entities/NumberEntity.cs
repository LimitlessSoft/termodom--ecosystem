using LSCore.Repository.Contracts;

namespace TD.Office.MassSMS.Contracts.Entities;

public class NumberEntity : LSCoreEntity
{
	public string Number { get; set; }
	public bool IsBlacklisted { get; set; }
}
