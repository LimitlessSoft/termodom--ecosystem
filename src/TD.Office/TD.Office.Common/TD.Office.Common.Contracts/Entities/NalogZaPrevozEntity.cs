using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.Entities;

public class NalogZaPrevozEntity : LSCoreEntity
{
	public string Mobilni { get; set; }
	public decimal CenaPrevozaBezPdv { get; set; }
	public decimal MiNaplatiliKupcuBezPdv { get; set; }
	public string? Note { get; set; }
	public string Address { get; set; }
	public int? VrDok { get; set; }
	public int? BrDok { get; set; }
	public int StoreId { get; set; }
	public string Prevoznik { get; set; }
	public bool PlacenVirmanom { get; set; }
	public NalogZaPrevozStatus Status { get; set; }
}
