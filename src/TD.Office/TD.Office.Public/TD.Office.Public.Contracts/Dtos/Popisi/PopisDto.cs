using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Dtos.Popisi;

public class PopisDto
{
	public long Id { get; set; }
	public string BrojDokumenta { get; set; } = string.Empty;
	public DateTime Datum { get; set; }
	public string Magacin { get; set; } = string.Empty;
	public DokumentStatus Status { get; set; }
	public int KomercijalnoPopisBrDok { get; set; }
	public int? KomercijalnoNarudzbenicaBrDok { get; set; }
}
