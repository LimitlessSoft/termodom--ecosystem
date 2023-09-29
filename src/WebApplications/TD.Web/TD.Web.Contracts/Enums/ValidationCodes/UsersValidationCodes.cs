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
        [Description("Korisnicko ime moze sadrzati samo slova i brojeve!")]
        UVC_007,
        [Description("Lozinka mora imati minimim {0} karaktera")]
        UVC_008,
        [Description("Lozinka moze imati maksimum {0} karaktera")]
        UVC_009,
        [Description("Lozinka mora sadrzati najmanje jedan broj i jedno slovo")]
        UVC_010,
        [Description("Morate proslediti nadimak!")]
        UVC_011,
        [Description("Nadimak mora imati minimim {0} karaktera")]
        UVC_012,
        [Description("Nadimak moze imati maksimum {0} karaktera")]
        UVC_013,
    }
}
