using TD.Web.Common.Contracts.Enums;
using LSCore.Contracts.Entities;

namespace TD.Web.Common.Contracts.Entities;

public class SettingEntity : LSCoreEntity
{
    public string Key { get; set; } // change to SettingKey
    public string Value { get; set; }
}