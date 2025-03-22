using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Repository.Contracts;

namespace TD.Web.Common.Contracts.Entities;

[Table("Stores")]
public class StoreEntity : LSCoreEntity
{
	public string Name { get; set; }
	public int? VPMagacinId { get; set; }

	[NotMapped]
	public List<UserEntity> Users { get; set; }
}
