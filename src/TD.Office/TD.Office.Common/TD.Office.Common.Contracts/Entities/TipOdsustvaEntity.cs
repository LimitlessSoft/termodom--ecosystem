using LSCore.Repository.Contracts;

namespace TD.Office.Common.Contracts.Entities;

public class TipOdsustvaEntity : LSCoreEntity
{
	public string Naziv { get; set; } = string.Empty;
	public int Redosled { get; set; }
}
