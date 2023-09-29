using System.ComponentModel;

namespace TD.Web.Contracts.Enums.ValidationCodes
{
    public enum UsersValidationCodes
    {
        [Description("Morate proslediti korisnicko ime!")]
        UVC_001,
        [Description("Korisnicko ime je vec zauzeto!")]
        UVC_002,
        [Description("Morate proslediti sifru!")]
        UVC_003,
        [Description("Korisnicko ime mora imati minimum {0} karaktera!")]
        UVC_004,
        [Description("Korisnicko ime moze imati maksimum {0} karaktera!")]
        UVC_005,
        [Description("Pogresno korisnicko ime ili lozinka!")]
        UVC_006,
    }
}
