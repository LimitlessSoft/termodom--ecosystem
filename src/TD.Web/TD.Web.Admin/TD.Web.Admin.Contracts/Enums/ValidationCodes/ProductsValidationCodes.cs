using System.ComponentModel;

namespace TD.Web.Admin.Contracts.Enums.ValidationCodes
{
    public enum ProductsValidationCodes
    {
        [Description("Proizvod sa istim imenom već postoji! Koristite drugo ime!")]
        PVC_001,
        [Description("Proizvod sa datim Id-em ne postoji!")]
        PVC_002,
        [Description("Proizvod '{0}' mora sadržati samo stova, brojeve, crtice ('-') i ne sme počinjati niti se završavati crticama ('-')")]
        PVC_003,
        [Description("'{0}' vrednost već postoji u bazi! Prosledite drugu vrednost ili promenite naziv (ako generisete {0} automatski)")]
        PVC_004,
        [Description("Unit nije pronadjen!")]
        PVC_005,
        [Description("ProductPriceGroup nije pronadjena!")]
        PVC_006,
    }
}
