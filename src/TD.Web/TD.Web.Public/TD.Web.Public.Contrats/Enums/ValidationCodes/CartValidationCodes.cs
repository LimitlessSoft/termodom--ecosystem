using System.ComponentModel;

namespace TD.Web.Public.Contracts.Enums.ValidationCodes
{
    public enum CartValidationCodes
    {
        [Description("Prodavnica ne postoji.")]
        CVC_001,
        [Description("{0} mora biti duže od {1} karaktera.")]
        CVC_002,
        [Description("{0} mora biti kraće od {1} karaktera.")]
        CVC_003,
        [Description("Način plaćanja ne postoji.")]
        CVC_004,
        [Description("'{0}' je obavezno polje.")]
        CVC_005,
    }
}
