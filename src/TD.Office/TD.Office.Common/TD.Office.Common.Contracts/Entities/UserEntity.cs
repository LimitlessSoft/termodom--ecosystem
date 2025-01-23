using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Contracts.Entities;
using LSCore.Contracts.Interfaces;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.Entities;

public class UserEntity : LSCoreEntity, ILSCoreAuthorizable
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string? RefreshToken { get; set; }
    public string Nickname { get; set; }
    public UserType Type { get; set; }
    public int? StoreId { get; set; }
    public int? VPMagacinId { get; set; }
    public decimal MaxRabatMPDokumenti { get; set; }
    public decimal MaxRabatVPDokumenti { get; set; }
    public int? KomercijalnoNalogId { get; set; }
    public long? LastNoteId { get; set; }

    [NotMapped]
    public List<UserPermissionEntity>? Permissions { get; set; }
}
