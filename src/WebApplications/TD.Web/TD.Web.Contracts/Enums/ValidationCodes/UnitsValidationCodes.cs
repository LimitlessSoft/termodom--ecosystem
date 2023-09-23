using System.ComponentModel;

namespace TD.Web.Contracts.Enums.ValidationCodes
{
    public enum UnitsValidationCodes
    {
        [Description("Jedinica sa datim Id-em ne postoji!")]
        UVC_001,
        [Description("Polje '{0}' može sadržati samo slova i brojeve!")]
        UVC_002,
    }
}
