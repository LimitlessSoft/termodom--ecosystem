using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Contracts.Entities;

namespace TD.Office.InterneOtpremnice.Contracts.Entities;

public class InternaOtpremnicaItemEntity : LSCoreEntity
{
    public int RobaId { get; set; }
    public decimal Kolicina { get; set; }
    public long InternaOtpremnicaId { get; set; }
    
    [NotMapped]
    public InternaOtpremnicaEntity? InternaOtpremnica { get; set; }
}