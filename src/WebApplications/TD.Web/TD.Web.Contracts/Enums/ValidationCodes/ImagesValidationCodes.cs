using System.ComponentModel;

namespace TD.Web.Contracts.Enums.ValidationCodes
{
    public enum ImagesValidationCodes
    {
        [Description("Slika mora biti JPG, PNG ili JPEG formata!")]
        IVC_001,
        [Description("Alt vrednost ne moze da sadrzi specijalne karaktere!")]
        IVC_002,
    }
}
