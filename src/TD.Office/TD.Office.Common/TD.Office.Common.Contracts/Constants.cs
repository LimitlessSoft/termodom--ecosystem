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
        }

        public static class Jwt
        {
            public const string ConfigurationKey = "JWT_KEY";
            public const string ConfigurationIssuer = "JWT_ISSUER";
            public const string ConfigurationAudience = "JWT_AUDIENCE";
        }
    }
}
