namespace TD.Office.Public.Contracts.Dtos.Permissions;

public class PermissionDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsGranted { get; set; }
}