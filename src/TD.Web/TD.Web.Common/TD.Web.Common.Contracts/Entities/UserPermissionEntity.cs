using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Repository.Contracts;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Entities;

public class UserPermissionEntity : LSCoreEntity
{
	public Permission Permission { get; set; }
	public long UserId { get; set; }

	[NotMapped]
	public UserEntity? User { get; set; }
}
