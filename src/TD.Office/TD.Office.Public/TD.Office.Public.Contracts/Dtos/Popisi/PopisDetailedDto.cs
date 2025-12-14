using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Dtos.Popisi;

public class PopisDetailedDto
{
	public long Id { get; set; }
	public DateTime Date { get; set; }
	public PopisDokumentType Type { get; set; }
	public DokumentStatus Status { get; set; }
	public List<PopisItemDto> Items { get; set; } = [];
	public int KomercijalnoPopisBrDok { get; set; }
	public int? KomercijalnoNarudzbenicaBrDok { get; set; }
}

public class PopisItemDto
{
	public long Id { get; set; }
	public long RobaId { get; set; }
	public string Naziv { get; set; } = string.Empty;

	public double PopisanaKolicina { get; set; }

	public double? NarucenaKolicina { get; set; }

	public DateTime LastChange { get; set; }
}
