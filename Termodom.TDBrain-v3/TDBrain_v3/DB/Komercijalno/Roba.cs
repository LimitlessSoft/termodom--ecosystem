using FirebirdSql.Data.FirebirdClient;
using System.Collections;
using System.Collections.Generic;

namespace TDBrain_v3.DB.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    public class Roba
    {
        /// <summary>
        /// 
        /// </summary>
        public class RobaCollection : IEnumerable<Roba>
        {
            private Dictionary<int, Roba> _dict { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="robaId"></param>
            /// <returns></returns>
            /// <exception cref="KeyNotFoundException"></exception>
            public Roba this[int robaId]
            {
                get
                {
                    return _dict[robaId];
                }
            }
            /// <summary>
            /// Kreira kolekciju robe
            /// </summary>
            /// <param name="dict"></param>
            public RobaCollection(Dictionary<int, Roba> dict)
            {
                _dict = dict;
            }

            /// <summary>
            /// Vraca enumerator nad vrednostima kolekcije
            /// </summary>
            /// <returns></returns>
            public IEnumerator<Roba> GetEnumerator()
            {
                return _dict.Values.GetEnumerator();
            }

            /// <summary>
            /// Vraca enumerator nad vrednostima kolekcije
            /// </summary>
            /// <returns></returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return _dict.Values.GetEnumerator();
            }
        }
        #region Properties
        
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string? KatBr { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string? KatBrPro { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string? Naziv { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string? JM { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string? TarifaID { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int Vrsta { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string? NazivZaStampu { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string? GrupaID { get; set; } = null;
        
        /// <summary>
        /// 
        /// </summary>
        public int? Podgrupa { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string? ProID { get; set; } = null;
        
        /// <summary>
        /// 
        /// </summary>
        public int? DOB_PPID { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string? TrPak { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? TrKol { get; set; }
        #endregion

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
        public static Roba Get(int magacinID, int godina, int robaID)
        {
            using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[magacinID, godina]))
            {
                con.Open();
                return Get(con, robaID);
            }
        }
        public static Roba Get(FbConnection con, int robaID)
        {
            using(FbCommand cmd = new FbCommand("SELECT * FROM ROBA WHERE ROBAID = @ROBAID", con))
            {
                cmd.Parameters.AddWithValue("@ROBAID", robaID);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new Roba()
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
        /// Vraca kolekciju robe iz baze u kojoj se nalazi Main magacin
        /// </summary>
        /// <param name="godina"></param>
        /// <returns></returns>
        public static RobaCollection Collection(int godina)
        {
            Dictionary<int, Roba> dict = new Dictionary<int, Roba>();
            using(FbConnection con = new FbConnection(Settings.ConnectionStringKomercijalno[Settings.MainMagacinKomercijalno, godina]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT * FROM ROBA", con))
                {
                    using (FbDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            dict.Add(Convert.ToInt32(dr["ROBAID"]), new Roba()
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

            return new RobaCollection(dict);
        }
        /// <summary>
        /// Vraca kolekciju robe iz baze prosledjenog connection string-a
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static RobaCollection Collection(FbConnection con)
        {
            Dictionary<int, Roba> dict = new Dictionary<int, Roba>();
            using (FbCommand cmd = new FbCommand("SELECT * FROM ROBA", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        dict.Add(Convert.ToInt32(dr["ROBAID"]), new Roba()
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

            return new RobaCollection(dict);
        }
    }
}
