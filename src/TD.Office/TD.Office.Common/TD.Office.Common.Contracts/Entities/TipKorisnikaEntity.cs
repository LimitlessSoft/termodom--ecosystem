using LSCore.Repository.Contracts;

namespace TD.Office.Common.Contracts.Entities;

public class TipKorisnikaEntity : LSCoreEntity
{
	public string Naziv { get; set; } = string.Empty;
	public string Boja { get; set; } = string.Empty;
}
