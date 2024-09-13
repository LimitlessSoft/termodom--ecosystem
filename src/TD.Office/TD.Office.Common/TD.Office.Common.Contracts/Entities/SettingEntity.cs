using LSCore.Contracts.Entities;

namespace TD.Office.Common.Contracts.Entities
{
    public class SettingEntity : LSCoreEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
