using System.ComponentModel;

namespace TD.Web.Admin.Contracts.Enums.ValidationCodes
{
    public enum ProductsGroupsValidationCodes
    {
        [Description("Roditeljska grupa ne postoji.")]
        PGVC_001,
        [Description("Naziv grupe proizvoda već postoji!")]
        PGVC_002,
        [Description("Roditeljska grupa je neispravna.")]
        PGVC_003,
        [Description("Ne možete obrisati grupu jer se koristi u nekoj drugoj grupi.")]
        PGVC_004,
        [Description("Ne možete obrisati grupu jer se koristi u nekom proizvodu.")]
        PGVC_005,
    }
}
