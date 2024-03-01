using System.ComponentModel;

namespace TD.Web.Public.Contracts.Enums.ValidationCodes
{
    public enum CartValidationCodes
    {
        [Description("Prodavnica ne postoji.")]
        CVC_001,
        [Description("Ime i prezime mora biti kraće od {0} karaktera.")]
        CVC_002,
        [Description("Napomena mora biti kraća od {0} karaktera.")]
        CVC_003,
        [Description("Način plaćanja ne postoji.")]
        CVC_004,
        [Description("'{0}' je obavezno polje.")]
        CVC_005,
        [Description("Ime i prezime mora biti duže od {0} karaktera")]
        CVC_006,
        [Description("Mobilni mora biti kraći od {0} karaktera")]
        CVC_007,
    }
}
