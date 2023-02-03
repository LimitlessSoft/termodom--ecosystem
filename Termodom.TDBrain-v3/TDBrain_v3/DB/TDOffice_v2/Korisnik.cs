using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
using System.Collections;
using System.Text;

namespace TDBrain_v3.DB.TDOffice_v2
{
    /// <summary>
    /// Klasa za komunikaciju sa tabelom korisnika
    /// </summary>
    public class Korisnik
    {
        public enum TipAutomatskogObavestenja
        {
            NakonRazduzenjaRobe = 0,
            NakonZameneRobe = 1,
            NakonZakljucavanjaPopisa = 2,
            NakonKreiranjaNovogPartnera = 3,
            PravoPristupaModulu = 4,
            PrimaObavestenjaOObnoviBonusaZakljucavanja = 5
        }
        public class Beleska
        {
            public int ID { get; set; }
            public string Body { get; set; }
            public string Naziv { get; set; }

        }
        public class Info
        {
            public int narudbenicaPPID { get; set; } = 4698;
            public string Beleska { get; set; }
            public List<Beleska> Beleske { get; set; } = new List<Beleska>();
            public Dictionary<TipAutomatskogObavestenja, bool> PrimaObavestenja = new Dictionary<TipAutomatskogObavestenja, bool>()
            {
                { TipAutomatskogObavestenja.NakonZameneRobe, false },
                { TipAutomatskogObavestenja.NakonRazduzenjaRobe, false },
                { TipAutomatskogObavestenja.PravoPristupaModulu, false }
            };
        }
        public class KorisnikCollection : IEnumerable<Korisnik>
        {
            private Dictionary<int, Korisnik> _dict { get; set; }

            public Korisnik this[int id] => _dict[id];

            public KorisnikCollection(Dictionary<int, Korisnik> dict) => _dict = dict;

            public IEnumerator<Korisnik> GetEnumerator() => _dict.Values.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #region Properties
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int MagacinID { get; set; }
        public object Tag { get; set; }

        public int? KomercijalnoUserID { get; set; }
        public int Grad { get; set; }
        public bool OpomeniZaNeizvrseneZadatke { get; set; }
        public int BonusZakljucavanjaCount { get; set; }
        public double BonusZakljucavanjaLimit { get; set; }
        #endregion
         
        public static KorisnikCollection Collection()
        {
            using(FbConnection con = new FbConnection(Settings.ConnectionStringTDOffice_v2.ConnectionString()))
            {
                con.Open();
                return Collection(con);
            }
        }
        public static KorisnikCollection Collection(FbConnection con)
        {
            Dictionary<int, Korisnik> dict = new Dictionary<int, Korisnik>();

            using(FbCommand cmd = new FbCommand("SELECT * FROM USERS", con))
            {
                using(FbDataReader dr = cmd.ExecuteReader())
                {
                    while(dr.Read())
                    {
                        dict.Add(Convert.ToInt32(dr["ID"]), new Korisnik()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Username = dr["USERNAME"].ToString(),
                            Password = dr["PW"].ToString(),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            KomercijalnoUserID = dr["KOMERCIJALNO_USER_ID"] is DBNull ? null : (int?)Convert.ToInt32(dr["KOMERCIJALNO_USER_ID"]),
                            Grad = Convert.ToInt32(dr["GRAD"]),
                            Tag = dr["TAG"] is DBNull ? new Info() : JsonConvert.DeserializeObject<Info>(Encoding.UTF8.GetString((byte[])dr["TAG"])),
                            OpomeniZaNeizvrseneZadatke = Convert.ToInt32(dr["OPOMENI_ZA_NEIZVRSENI_ZADATAK"]) == 1,
                            BonusZakljucavanjaCount = Convert.ToInt32(dr["BONUS_ZAKLJUCAVANJA_COUNT"]),
                            BonusZakljucavanjaLimit = Convert.ToDouble(dr["BONUS_ZAKLJUCAVANJA_LIMIT"])
                        });
                    }
                }
            }

            return new KorisnikCollection(dict);
        }
    }
}
