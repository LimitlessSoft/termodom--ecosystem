using LSCore.Repository.Contracts;

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
}
