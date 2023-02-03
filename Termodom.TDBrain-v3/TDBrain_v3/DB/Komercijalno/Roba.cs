using FirebirdSql.Data.FirebirdClient;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.DB.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    public class Roba
    {
        /// <summary>
        /// Azurira podatke Robe u bazi
        /// </summary>
        /// <param name="con">Konekcija do baze nad kojom se vrsi azuriranje</param>
        /// <param name="robaID">Unikatni identifikator objekta</param>
        /// <param name="katBr"></param>
        /// <param name="katBrPro"></param>
        /// <param name="naziv"></param>
        /// <param name="vrsta"></param>
        /// <param name="grupaID"></param>
        /// <param name="podgrupaID"></param>
        /// <param name="proID"></param>
        /// <param name="jm"></param>
        /// <param name="tarifaID"></param>
        /// <param name="trPakJM"></param>
        /// <param name="trPakKol"></param>
        public static void Update(FbConnection con, int robaID, string katBr, string katBrPro, string naziv, int vrsta, string grupaID,
            int podgrupaID, string proID, string jm, string tarifaID, string? trPakJM, double? trPakKol)
        {
            using(FbCommand cmd = new FbCommand(@"UPDATE ROBA SET
KATBR = @KATBR,
KATBRPRO = @KATBRPRO,
NAZIV = @NAZIV,
VRSTA = @VRSTA,
AKTIVNA = @AKTIVNA,
GRUPAID = @GRUPAID,
PODGRUPA = @PODGRUPA,
PROID = @PROID,
JM = @JM,
TARIFAID = @TARIFAID,
ALTJM = @TRPAK,
TRKOL = @TRKOL
WHERE
ROBAID = @ROBAID", con))
            {
                cmd.Parameters.AddWithValue("@KATBR", katBr);
                cmd.Parameters.AddWithValue("@KATBRPRO", katBrPro);
                cmd.Parameters.AddWithValue("@NAZIV", naziv);
                cmd.Parameters.AddWithValue("@VRSTA", vrsta);
                cmd.Parameters.AddWithValue("@AKTIVNA", 1);
                cmd.Parameters.AddWithValue("@GRUPAID", grupaID);
                cmd.Parameters.AddWithValue("@PODGRUPA", podgrupaID);
                cmd.Parameters.AddWithValue("@PROID", proID);
                cmd.Parameters.AddWithValue("@JM", jm);
                cmd.Parameters.AddWithValue("@TARIFAID", tarifaID);
                cmd.Parameters.AddWithValue("@TRPAK", trPakJM);
                cmd.Parameters.AddWithValue("@TRKOL", trPakKol);
                cmd.Parameters.AddWithValue("@ROBAID", robaID);

                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Insertuje novi objekat Robe u bazu i vraca novi ID
        /// </summary>
        /// <param name="con">Konekcija do baze nad kojom se vrsi insertovanje</param>
        /// <param name="katBr"></param>
        /// <param name="katBrPro"></param>
        /// <param name="naziv"></param>
        /// <param name="vrsta"></param>
        /// <param name="grupaID"></param>
        /// <param name="podgrupaID"></param>
        /// <param name="proID"></param>
        /// <param name="jm"></param>
        /// <param name="tarifaID"></param>
        /// <param name="trPakJM"></param>
        /// <param name="trPakKol"></param>
        /// <returns>Novokreirani ID objekta</returns>
        public static int Insert(FbConnection con, string katBr, string katBrPro, string naziv, int vrsta, string grupaID, int podgrupaID, string proID,
            string jm, string tarifaID, string? trPakJM, double? trPakKol)
        {
            using(FbCommand cmd = new FbCommand(@"INSERT INTO ROBA (ROBAID, KATBR, KATBRPRO, NAZIV,
VRSTA, AKTIVNA, GRUPAID, PODGRUPA, PROID, JM, TARIFAID, ALTJM, TRKOL) VALUES (((SELECT COALESCE(MAX(ROBAID), 0) FROM ROBA) + 1),@KATBR, @KATBRPRO, @NAZIV,
@VRSTA, @AKTIVNA, @GRUPAID, @PODGRUPA, @PROID, @JM, @TARIFAID, @TRPAK, @TRKOL) RETURNING ROBAID", con))
            {
                cmd.Parameters.Add(new FbParameter("ROBAID", FbDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.AddWithValue("@KATBR", katBr);
                cmd.Parameters.AddWithValue("@KATBRPRO", katBrPro);
                cmd.Parameters.AddWithValue("@NAZIV", naziv);
                cmd.Parameters.AddWithValue("@VRSTA", vrsta);
                cmd.Parameters.AddWithValue("@AKTIVNA", 1);
                cmd.Parameters.AddWithValue("@GRUPAID", grupaID);
                cmd.Parameters.AddWithValue("@PODGRUPA", podgrupaID);
                cmd.Parameters.AddWithValue("@PROID", proID);
                cmd.Parameters.AddWithValue("@JM", jm);
                cmd.Parameters.AddWithValue("@TARIFAID", tarifaID);
                cmd.Parameters.AddWithValue("@TRPAK", trPakJM);
                cmd.Parameters.AddWithValue("@TRKOL", trPakKol);

                cmd.ExecuteNonQuery();
                return Convert.ToInt32(cmd.Parameters["ROBAID"].Value);
            }
        }
        /// <summary>
        /// Vraca podatke o objektu iz baze trenutne godine, defaultnog magacina
        /// </summary>
        /// <param name="robaID"></param>
        /// <returns></returns>
        public static Termodom.Data.Entities.Komercijalno.Roba? Get(int robaID)
        {
            return Get(robaID, DateTime.Now.Year);
        }
        /// <summary>
        /// Vraca podatke o objektu iz baze izabrane godine, defaultnog magacina
        /// </summary>
        /// <param name="robaID"></param>
        /// <param name="godina"></param>
        /// <returns></returns>
        public static Termodom.Data.Entities.Komercijalno.Roba? Get(int robaID, int godina)
        {
            return Get(robaID, godina, Settings.MainMagacinKomercijalno);
        }
        /// <summary>
        /// Vraca podatke o objektu iz baze izabrane godine, izabranog magacina
        /// </summary>
        /// <param name="robaID">ID objekta</param>
        /// <param name="godina">Godina za koju se vrsi selektovanje objekta</param>
        /// <param name="magacinID">MagacinID nad kojim se vrsi selektovanje objekta</param>
        /// <returns></returns>
        public static Termodom.Data.Entities.Komercijalno.Roba? Get(int robaID, int godina, int magacinID)
        {
            using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[magacinID, godina]))
            {
                con.Open();
                return Get(con, robaID);
            }
        }
        /// <summary>
        /// Vraca podatke o objektu iz baze prosledjene konekcije
        /// </summary>
        /// <param name="con">Konekcija baze nad kojom se vrsi selektovanje</param>
        /// <param name="robaID">ID robe koja se trazi</param>
        /// <returns></returns>
        public static Termodom.Data.Entities.Komercijalno.Roba? Get(FbConnection con, int robaID)
        {
            using(FbCommand cmd = new FbCommand("SELECT * FROM ROBA WHERE ROBAID = @ROBAID", con))
            {
                cmd.Parameters.AddWithValue("@ROBAID", robaID);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new Termodom.Data.Entities.Komercijalno.Roba()
                        {
                            ID = Convert.ToInt32(dr["ROBAID"]),
                            Naziv = dr["NAZIV"].ToString(),
                            KatBr = dr["KATBR"].ToString(),
                            KatBrPro = dr["KATBRPRO"].ToString(),
                            JM = dr["JM"].ToString(),
                            TarifaID = dr["TARIFAID"].ToString(),
                            Vrsta = Convert.ToInt32(dr["VRSTA"]),
                            NazivZaStampu = dr["NAZIVZASTAMPU"].ToString(),
                            TrKol = dr["TRKOL"] is DBNull ? null : (double?)Convert.ToDouble(dr["TRKOL"]),
                            GrupaID = dr["GRUPAID"].ToString(),
                            Podgrupa = dr["PODGRUPA"] is DBNull ? null : (int?)Convert.ToInt32(dr["PODGRUPA"]),
                            ProID = dr["PROID"].ToString(),
                            DOB_PPID = dr["DOB_PPID"] is DBNull ? null : (int?)Convert.ToInt32(dr["DOB_PPID"]),
                            TrPak = dr["ALTJM"] is DBNull ? null : dr["ALTJM"].ToString()
                        };
            }
            return null;
        }
        /// <summary>
        /// Vraca dictionary robe iz baze izabrane godine, defaultnog magacina
        /// </summary>
        /// <param name="godina"></param>
        /// <returns></returns>
        public static RobaDictionary Collection(int godina)
        {
            Dictionary<int, Termodom.Data.Entities.Komercijalno.Roba> dict = new Dictionary<int, Termodom.Data.Entities.Komercijalno.Roba>();
            using(FbConnection con = new FbConnection(Settings.ConnectionStringKomercijalno[Settings.MainMagacinKomercijalno, godina]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT * FROM ROBA", con))
                {
                    using (FbDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            dict.Add(Convert.ToInt32(dr["ROBAID"]), new Termodom.Data.Entities.Komercijalno.Roba()
                            {
                                ID = Convert.ToInt32(dr["ROBAID"]),
                                Naziv = dr["NAZIV"].ToString(),
                                KatBr = dr["KATBR"].ToString(),
                                KatBrPro = dr["KATBRPRO"].ToString(),
                                JM = dr["JM"].ToString(),
                                TarifaID = dr["TARIFAID"].ToString(),
                                Vrsta = Convert.ToInt32(dr["VRSTA"]),
                                NazivZaStampu = dr["NAZIVZASTAMPU"].ToString(),
                                TrKol = dr["TRKOL"] is DBNull ? null : (double?)Convert.ToDouble(dr["TRKOL"]),
                                GrupaID = dr["GRUPAID"].ToString(),
                                Podgrupa = dr["PODGRUPA"] is DBNull ? null : (int?)Convert.ToInt32(dr["PODGRUPA"]),
                                ProID = dr["PROID"].ToString(),
                                DOB_PPID = dr["DOB_PPID"] is DBNull ? null : (int?)Convert.ToInt32(dr["DOB_PPID"]),
                                TrPak = dr["ALTJM"] is DBNull ? null : dr["ALTJM"].ToString()
                            });
                }
            }

            return new RobaDictionary(dict);
        }
        /// <summary>
        /// Vraca dictionary robe iz baze prosledjene konekcije
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static RobaDictionary Collection(FbConnection con)
        {
            Dictionary<int, Termodom.Data.Entities.Komercijalno.Roba> dict = new Dictionary<int, Termodom.Data.Entities.Komercijalno.Roba>();
            using (FbCommand cmd = new FbCommand("SELECT * FROM ROBA", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        dict.Add(Convert.ToInt32(dr["ROBAID"]), new Termodom.Data.Entities.Komercijalno.Roba()
                        {
                            ID = Convert.ToInt32(dr["ROBAID"]),
                            Naziv = dr["NAZIV"].ToString(),
                            KatBr = dr["KATBR"].ToString(),
                            KatBrPro = dr["KATBRPRO"].ToString(),
                            JM = dr["JM"].ToString(),
                            TarifaID = dr["TARIFAID"].ToString(),
                            Vrsta = Convert.ToInt32(dr["VRSTA"]),
                            NazivZaStampu = dr["NAZIVZASTAMPU"].ToString(),
                            TrKol = dr["TRKOL"] is DBNull ? null : (double?)Convert.ToDouble(dr["TRKOL"]),
                            GrupaID = dr["GRUPAID"].ToString(),
                            Podgrupa = dr["PODGRUPA"] is DBNull ? null : (int?)Convert.ToInt32(dr["PODGRUPA"]),
                            ProID = dr["PROID"].ToString(),
                            DOB_PPID = dr["DOB_PPID"] is DBNull ? null : (int?)Convert.ToInt32(dr["DOB_PPID"]),
                            TrPak = dr["ALTJM"] is DBNull ? null : dr["ALTJM"].ToString()
                        });
            }

            return new RobaDictionary(dict);
        }
    }
}
