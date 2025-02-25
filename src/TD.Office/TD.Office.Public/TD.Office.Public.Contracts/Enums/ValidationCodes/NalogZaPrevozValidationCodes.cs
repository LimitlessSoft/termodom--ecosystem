using System.ComponentModel;

namespace TD.Office.Public.Contracts.Enums.ValidationCodes;
public enum NalogZaPrevozValidationCodes
{
    [Description("Napomena je obavezno polje za osnov 'Ostalo'")]
    NZPVC_001,
    [Description("Neispravan osnov")]
    NZPVC_002,
    [Description("Neispravan broj dokumenta")]
    NZPVC_003,
}
