namespace TD.Office.Public.Contracts;

public static class LegacyConstants
{
    public static int NalogZaUtovarDefaultNUID = 18;
    public static int ProracunDefaultNUID = 5;
    public static int ProfakturaDefaultNUID = 1;
    public const string KomercijalnoApiUrlFormat = "https://{0}-komercijalno{1}.termodom.rs";
    public const int AzurirajCeneUslovFormiranjaMinWebOsnovaProductSuggestionSearchTextMinimumLength =
        4;
    public const string PartnerIzvestajFinansijskoKomercijalnoLabelFormat = "TCMDZ {0}";

    public const string ProracunRobaNotFoundText = "Roba u Komercijalnom nije pronađena";
    public static readonly int[] DefaultPartnerIzvestajKomercijalnoDokumenti =
    {
        10,
        13,
        14,
        15,
        22,
        39,
        40
    };

    public const int MinRabatMPDokumenti = 0;
    public const int MaxRabatMPDokumenti = 100;

    public const int MinRabatVPDokumenti = 0;
    public const int MaxRabatVPDokumenti = 100;
}
