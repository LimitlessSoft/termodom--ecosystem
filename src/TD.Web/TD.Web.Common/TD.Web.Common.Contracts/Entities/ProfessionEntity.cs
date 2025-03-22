using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Repository.Contracts;

namespace TD.Web.Common.Contracts.Entities;

public class ProfessionEntity : LSCoreEntity
{
	public string Name { get; set; }

	[NotMapped]
	public List<UserEntity> Users { get; set; }
}
