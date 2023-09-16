using System.ComponentModel;

namespace TD.Web.Contracts.Enums.ValidationCodes
{
    public enum ProductsValidationCodes
    {
        [Description("Product with same name already exists! Use another name!")]
        PVC_001,
        [Description("Product with given Id doesn't exist!")]
        PVC_002,
        [Description("Product 'Src' is not valid!")]
        PVC_003,
        [Description("'Src' vrednost već postoji u bazi! Prosledite drugu vrednost ili promenite naziv (ako generisete src automatski)")]
        PVC_004
    }
}
