using LSCore.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.Entities;

public class ProracunEntity : LSCoreEntity
{
    public int MagacinId { get; set; }
    public ProracunState State { get; set; }
    public ProracunType Type { get; set; }
    public List<ProracunItemEntity> Items { get; set; }
}
