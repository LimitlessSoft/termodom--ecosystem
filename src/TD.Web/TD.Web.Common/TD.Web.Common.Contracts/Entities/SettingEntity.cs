using LSCore.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Entities
{
    public class SettingEntity : LSCoreEntity
    {
        public Setting Key { get; set; }
        public string Value { get; set; }
    }
}
