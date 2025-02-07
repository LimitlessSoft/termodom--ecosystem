using System.ComponentModel;

namespace TD.Office.Public.Contracts.Enums.ValidationCodes;
public enum NalogZaPrevozValidationCodes
{
    [Description("Napomena je obavezno polje")]
    NZPVC_001,
    [Description("'{0}' mora biti vece od 0")]
    NZPVC_002,
}
