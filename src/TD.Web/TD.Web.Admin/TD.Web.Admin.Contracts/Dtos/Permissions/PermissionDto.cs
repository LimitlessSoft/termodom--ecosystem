namespace TD.Web.Admin.Contracts.Dtos.Permissions;

public class PermissionDto
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsGranted { get; set; }
}