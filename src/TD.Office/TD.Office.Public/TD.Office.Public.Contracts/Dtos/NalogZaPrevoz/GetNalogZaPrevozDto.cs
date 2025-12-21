using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Dtos.NalogZaPrevoz;

public class GetNalogZaPrevozDto
{
	public long Id { get; set; }
	public string Mobilni { get; set; }
	public decimal CenaPrevozaBezPdv { get; set; }
	public decimal MiNaplatiliKupcuBezPdv { get; set; }
	public string Note { get; set; }
	public string Address { get; set; }
	public int? VrDok { get; set; }
	public int? BrDok { get; set; }
	public int StoreId { get; set; }
	public DateTime CreatedAt { get; set; }
	public string Prevoznik { get; set; }
	public bool PlacenVirmanom { get; set; }
	public NalogZaPrevozStatus Status { get; set; }
}
