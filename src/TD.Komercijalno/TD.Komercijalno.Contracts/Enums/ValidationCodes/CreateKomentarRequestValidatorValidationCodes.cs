using System.ComponentModel;

namespace TD.Komercijalno.Contracts.Enums.ValidationCodes
{
    public enum CreateKomentarRequestValidatorValidationCodes
    {
        [Description("Jedan od komentara mora imati vrednost ({0} ili {1} ili {2})")]
        CKRV_001,
        [Description("Komentari na ovom dokumentu vec postoje. Koristi PUT da azuriras")]
        CKRV_002
    }
}
