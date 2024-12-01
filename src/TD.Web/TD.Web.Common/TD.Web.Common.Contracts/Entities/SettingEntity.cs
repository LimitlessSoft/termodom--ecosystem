using TD.Web.Common.Contracts.Enums;
using LSCore.Contracts.Entities;

namespace TD.Web.Common.Contracts.Entities;

public class SettingEntity : LSCoreEntity
{
    public SettingKey Key { get; set; }
    public string Value { get; set; }
}