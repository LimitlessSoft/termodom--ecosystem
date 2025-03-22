using LSCore.Repository.Contracts;

namespace TD.Web.Common.Contracts.Entities;

public class KomercijalnoWebProductLinkEntity : LSCoreEntity
{
	public int RobaId { get; set; }
	public long WebId { get; set; }
}
