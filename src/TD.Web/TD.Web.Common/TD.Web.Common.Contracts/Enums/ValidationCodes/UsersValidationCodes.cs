using System.ComponentModel;

namespace TD.Web.Common.Contracts.Enums.ValidationCodes
{
    public enum UsersValidationCodes
    {
        [Description("Morate proslediti korisničko ime!")]
        UVC_001,
        [Description("Korisničko ime je već zauzeto!")]
        UVC_002,
        [Description("Morate proslediti šifru!")]
        UVC_003,
        [Description("Korisničko ime mora imati minimum {0} karaktera.")]
        UVC_004,
        [Description("Korisničko ime moze imati maksimum {0} karaktera.")]
        UVC_005,
        [Description("Pogrešno korisničko ime ili lozinka.")]
        UVC_006,
        [Description("Korisničko ime može sadržati samo slova i brojeve.")]
        UVC_007,
        [Description("Lozinka mora imati minimim {0} karaktera.")]
        UVC_008,
        [Description("Lozinka moze imati maksimum {0} karaktera.")]
        UVC_009,
        [Description("Lozinka mora sadržati najmanje jedan broj i jedno slovo.")]
        UVC_010,
        [Description("Morate proslediti nadimak.")]
        UVC_011,
        [Description("Nadimak mora imati minimim {0} karaktera.")]
        UVC_012,
        [Description("Nadimak može imati maksimum {0} karaktera.")]
        UVC_013,
        [Description("Korisnici mogu imati izmedju 18 i 70 godina.")]
        UVC_014,
        [Description("Tip korisnika ne postoji.")]
        UVC_015,
        [Description("Korisnik nije pronađen.")]
        UVC_016,
        [Description("Vaš nalog još uvek nije verifikovan.")]
        UVC_017,
        [Description("Korisnik ne postoji.")]
        UVC_018,
        [Description("Cenovna grupa proizvoda ne postoji.")]
        UVC_019,
        [Description("Nivo mora biti veći ili jednak od 0 i manji od {0}.")]
        UVC_020,
        [Description("Morate proslediti Id")]
        UVC_021,
        [Description("Grad ne postoji")]
        UVC_022,
        [Description("Prodavnica ne postoji")]
        UVC_023,
        [Description("Zanimanje ne postoji")]
        UVC_024,
        [Description("PIB ne može biti duži od {0}")]
        UVC_025,
        [Description("Komentar ne može biti duži od {0}")]
        UVC_026,
        [Description("Korisnik ne postoji")]
        UVC_027,
        [Description("Korisnik sa tim mobilnim telefonom već postoji")]
        UVC_028,
        [Description("Vi ste u statusu upoznavanja, za mogućnost poručivanja pozovite 064-108-39-32 ili se izlogujte i poručite kao jednokratni kupac.")]
        UVC_029,
        [Description("Mobilni telefon nije validan")]
        UVC_030,
        [Description("Email nije validan")]
        UVC_031,
    }
}
