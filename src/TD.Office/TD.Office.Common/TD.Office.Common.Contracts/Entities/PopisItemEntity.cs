using LSCore.Repository.Contracts;

namespace TD.Office.Common.Contracts.Entities;

public class PopisItemEntity : LSCoreEntity
{
	public long PopisDokumentId { get; set; }
	public PopisDokumentEntity? Dokument { get; set; }
	public long RobaId { get; set; }
	public double? PopisanaKolicina { get; set; }
	public double? NarucenaKolicina { get; set; }
}
