using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.Entities;

public class UserPermissionEntity : LSCoreEntity
{
	public Permission Permission { get; set; }
	public long UserId { get; set; }

	[NotMapped]
	public UserEntity? User { get; set; }
}
