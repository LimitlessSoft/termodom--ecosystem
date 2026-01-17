using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.Entities;

public class OdsustvoEntity : LSCoreEntity
{
	public long UserId { get; set; }
	public UserEntity? User { get; set; }
	public long TipOdsustvaId { get; set; }
	public TipOdsustvaEntity? TipOdsustva { get; set; }
	public DateTime DatumOd { get; set; }
	public DateTime DatumDo { get; set; }
	public string? Komentar { get; set; }
	public OdsustvoStatus Status { get; set; }
	public DateTime? OdobrenoAt { get; set; }
	public long? OdobrenoBy { get; set; }
	public UserEntity? OdobrenoByUser { get; set; }
	public bool RealizovanoKorisnik { get; set; }
	public bool RealizovanoOdobravac { get; set; }
}
