namespace TD.Office.Public.Contracts
{
    public static class Constants
    {
        public const string KomercijalnoApiUrlFormat = "https://localhost:7205"; //change to prod later TODO:
        public const int AzurirajCeneUslovFormiranjaMinWebOsnovaProductSuggestionSearchTextMinimumLength = 4;
        public const string PartnerIzvestajFinansijskoKomercijalnoLabelFormat = "TCMDZ {0}";
        public static readonly int[] DefaultPartnerIzvestajKomercijalnoDokumenti = { 10, 13, 14, 15, 22, 39, 40 };
    }
}
