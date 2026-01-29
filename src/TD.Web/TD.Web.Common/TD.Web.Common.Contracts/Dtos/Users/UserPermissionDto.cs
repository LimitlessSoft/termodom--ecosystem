namespace TD.Web.Common.Contracts.Dtos.Users;

public class UserPermissionDto
{
	public required long Id { get; set; }
	public required string Name { get; set; }
	public required string Description { get; set; }
	public required bool IsGranted { get; set; }
}
