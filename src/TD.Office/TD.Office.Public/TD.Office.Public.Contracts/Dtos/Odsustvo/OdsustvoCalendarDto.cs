using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Dtos.Odsustvo;

public class OdsustvoCalendarDto
{
	public long Id { get; set; }
	public long UserId { get; set; }
	public string UserNickname { get; set; } = string.Empty;
	public long TipOdsustvaId { get; set; }
	public string TipOdsustvaNaziv { get; set; } = string.Empty;
	public DateTime DatumOd { get; set; }
	public DateTime DatumDo { get; set; }
	public string? Komentar { get; set; }
	public OdsustvoStatus Status { get; set; }
	public bool RealizovanoKorisnik { get; set; }
	public bool RealizovanoOdobravac { get; set; }
}
