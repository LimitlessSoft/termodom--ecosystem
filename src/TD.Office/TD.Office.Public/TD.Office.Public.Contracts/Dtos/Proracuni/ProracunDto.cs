using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Dtos.Proracuni;

public class ProracunDto
{
	public long Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public int MagacinId { get; set; }
	public ProracunState State { get; set; }
	public string Type { get; set; }
	public List<ProracunItemDto> Items { get; set; } = [];
	public string KomercijalnoDokument { get; set; }
	public string Referent { get; set; }
	public int? PPID { get; set; }
	public int NUID { get; set; }
	public string? Email { get; set; }
	public decimal UkupnoBezPdv =>
		Items.Sum(x => x.CenaBezPdv * ((100 - x.Rabat) / 100) * x.Kolicina);
	public decimal UkupnoPdv =>
		Items.Sum(x => x.CenaBezPdv * ((100 - x.Rabat) / 100) * x.Kolicina * x.Pdv / 100);
}
