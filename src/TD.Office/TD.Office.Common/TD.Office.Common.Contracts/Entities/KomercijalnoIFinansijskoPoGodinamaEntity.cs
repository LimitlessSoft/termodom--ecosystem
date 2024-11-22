using LSCore.Contracts.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Office.Common.Contracts.Entities;
public class KomercijalnoIFinansijskoPoGodinamaEntity : LSCoreEntity
{
    public int PPID { get; set; }
    public string? Komentar { get; set; }
    public long StatusId { get; set; }
    [NotMapped]
    public KomercijalnoIFinansijskoPoGodinamaStatusEntity Status { get; set; }
}
