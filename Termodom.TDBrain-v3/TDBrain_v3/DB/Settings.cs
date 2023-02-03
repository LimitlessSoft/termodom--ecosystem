namespace TDBrain_v3.DB
{
    public class Settings
    {
        public static string ServerName { get; set; } = "localhost";
        public static string Password { get; set; } = "masterkey";
        public static string User { get; set; } = "SYSDBA";
        public static SingleConnectionString ConnectionStringTDOffice_v2 { get; set; } = new SingleConnectionString();
        public static ConnectionStringCollection ConnectionStringKomercijalno { get; set; } = new ConnectionStringCollection();
        public static SingleConnectionString ConnectionStringKomercijalnoConfig { get; set; } = new SingleConnectionString();
        /// <summary>
        /// Ovo je glavni magacin komercijalnog poslovanja.
        /// Ovaj magacin diktira podatke kao sto su Roba, Partner itd i sinhronizuje ih sa ostalima
        /// Ukoliko dodje do konflikta, ovo je magacin koji je najstariji i iz njega se uziam podatak kao ispravan
        /// </summary>
        public static int MainMagacinKomercijalno { get; set; } = 150;
    }
}
