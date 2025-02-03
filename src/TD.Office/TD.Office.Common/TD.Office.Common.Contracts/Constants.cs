namespace TD.Office.Common.Contracts
{
    public static class Constants
    {
        public const string ProjectName = "TD.Office";
        public static class PermissionGroup
        {
            public const string NavBar = "nav-bar";
            public const string NalogZaPrevoz = "nalog-za-prevoz";
            public const string Korisnici = "korisnici";
            public const string Web = "web";
            public const string Partneri = "partneri";
            public const string SpecifikacijaNovca = "specifikacija-novca";
            public const string PartnerIzvestajFinansijskoKomercijalno =
                "partneri-izvestaj-finansijko-komercijalno";
            public const string IzvestajUkupneKolicinePoRobiUFiltriranimDokumentima =
                "izvestaj-ukupne-kolicine-po-robi-u-filtriranim-dokumentima";
            public const string Proracuni = "proracuni";
            public const string IzvestajIzlazaRobePoGodinama = "izvestaj-izlaza-robe-po-godinama";
            public const string PartnerAnaliza = "partneri-analiza";
        }

        public static class Jwt
        {
            public const string ConfigurationKey = "JWT_KEY";
            public const string ConfigurationIssuer = "JWT_ISSUER";
            public const string ConfigurationAudience = "JWT_AUDIENCE";
        }

        public static class DbMigrations
        {
            public static readonly string DbSeedsRoot = Path.Combine(
                Environment.CurrentDirectory,
                "DbSeeds"
            );
            public static readonly string DbSeedsDownRoot = Path.Combine(
                Environment.CurrentDirectory,
                "DbSeeds",
                "Down"
            );
        }

        public static class CacheKeys
        {
            public const string AzurirajCeneKomercijalnoPoslovanjeInprogressKey = "td-office-komercijalno-azuriranje-cena";
            public const string WebAuzurirajCeneMaxWebOsnoveInProgressKey = "td-office-azuriraj-cene-max-in-progress";
            public const string WebAuzurirajCeneMinWebOsnoveInProgressKey = "td-office-azuriraj-cene-min-in-progress";
        }
    }
}
