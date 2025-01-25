using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Contracts.Entities;
using TD.Office.InterneOtpremnice.Contracts.Enums;

namespace TD.Office.InterneOtpremnice.Contracts.Entities;

public class InternaOtpremnicaEntity : LSCoreEntity
{
    public InternaOtpremnicaStatus Status { get; set; }
    public int PolazniMagacinId { get; set; }
    public int DestinacioniMagacinId { get; set; }
    
    [NotMapped]
    public List<InternaOtpremnicaItemEntity>? Items { get; set; }
}
