using TD.Office.Common.Contracts.Enums;
using LSCore.Contracts.Requests;

namespace TD.Office.Public.Contracts.Requests.Users;

public class UsersUpdatePermissionRequest : LSCoreSaveRequest
{
    public Permission? Permission { get; set; }
    public bool IsGranted { get; set; }
}