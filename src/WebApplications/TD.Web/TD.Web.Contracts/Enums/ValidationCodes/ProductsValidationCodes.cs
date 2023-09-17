using System.ComponentModel;

namespace TD.Web.Contracts.Enums.ValidationCodes
{
    public enum ProductsValidationCodes
    {
        [Description("Product with same name already exists! Use another name!")]
        PVC_001,
        [Description("Product with given Id doesn't exist!")]
        PVC_002,
        [Description("Product '{0}' must contain only letters, numbers and dashes and not starts and ends with dash")]
        PVC_003,
        [Description("'{0}' vrednost već postoji u bazi! Prosledite drugu vrednost ili promenite naziv (ako generisete {0} automatski)")]
        PVC_004
    }
}
