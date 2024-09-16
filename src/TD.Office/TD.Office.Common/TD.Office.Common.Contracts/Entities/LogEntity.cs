using LSCore.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.Entities;

public class LogEntity : LSCoreEntity
{
    public LogKey Key { get; set; }
    public string? Value { get; set; }
}
