using TD.Web.Common.Contracts.Enums;
using LSCore.Contracts.Entities;

namespace TD.Web.Common.Contracts.Entities;

public class GlobalAlertEntity : LSCoreEntity
{
    public GlobalAlertApplication Application { get; set; }
    public string Text { get; set; }
}