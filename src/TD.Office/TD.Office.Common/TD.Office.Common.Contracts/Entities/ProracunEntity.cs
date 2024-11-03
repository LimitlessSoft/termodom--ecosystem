using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.Entities;

public class ProracunEntity : LSCoreEntity
{
    public int MagacinId { get; set; }
    public ProracunState State { get; set; }
    public ProracunType Type { get; set; }
    public List<ProracunItemEntity> Items { get; set; }
    public int? KomercijalnoVrDok { get; set; }
    public int? KomercijalnoBrDok { get; set; }
    public int? PPID { get; set; }
    public int NUID { get; set; }

    [NotMapped]
    public UserEntity User { get; set; }
}
