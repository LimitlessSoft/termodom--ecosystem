namespace TD.FE.TDOffice.Contracts
{
    public static class Constants
    {
        public static class KomercijalnoApiEndpoints
        {
            public static class Dokumenti
            {
                public static string Root { get; } = "/dokumenti";
                public static string Get { get; } = Root;
            }
        }

        public static class TDOfficeApiEndpoints
        {
            public static class DokumentTagIzvodi
            {
                public static string Root { get; } = "/dokument-tag-izvodi";
                public static string Get { get; } = Root;
                public static string Put { get; } = Root;
            }
        }
    }
}
