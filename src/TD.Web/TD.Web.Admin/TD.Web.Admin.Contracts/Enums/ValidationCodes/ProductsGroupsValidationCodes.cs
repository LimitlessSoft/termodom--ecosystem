using System.ComponentModel;

namespace TD.Web.Admin.Contracts.Enums.ValidationCodes
{
    public enum ProductsGroupsValidationCodes
    {
        [Description("Roditeljska grupa ne postoji.")]
        PGVC_001,
        [Description("Naziv proizvod grupe već postoji.")]
        PGVC_002,
        [Description("Roditeljska grupa je neispravna.")]
        PGVC_003,
    }
}
