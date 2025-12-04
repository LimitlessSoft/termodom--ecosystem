using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.Entities;

public class PopisDokumentEntity : LSCoreEntity
{
	public long MagacinId { get; set; }
	public PopisDokumentType Type { get; set; }
	public DokumentStatus Status { get; set; }
	public List<PopisItemEntity>? Items { get; set; }
	public UserEntity? User { get; set; }
}
