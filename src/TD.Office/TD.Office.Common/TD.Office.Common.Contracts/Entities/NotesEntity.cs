using LSCore.Contracts.Entities;

namespace TD.Office.Common.Contracts.Entities;
public class NotesEntity : LSCoreEntity
{
    public string Name { get; set; }
    public string? Content { get; set; }
}
