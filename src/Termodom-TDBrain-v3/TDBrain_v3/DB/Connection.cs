using FirebirdSql.Data.FirebirdClient;

namespace TDBrain_v3.DB
{
    public static class Connection
    {
        public static FbConnection TDOffice_v2 { get; set; }

        private static object TDOffice_v2_lock { get; set; }
        public static FbConnection GetTDOffice_v2Connection()
        {
            lock(TDOffice_v2_lock)
            {
                return TDOffice_v2;
            }
        }
    }
}
