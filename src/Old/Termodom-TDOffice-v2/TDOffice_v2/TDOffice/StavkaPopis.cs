using FirebirdSql.Data.FirebirdClient;
using LimitlessSoft.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public class StavkaPopis
    {
        private static Buffer<List<StavkaPopis>> _buffer = new Buffer<List<StavkaPopis>>(List);

        public int ID { get; set; }
        public int BrDok { get; set; }
        public double? Kolicina { get; set; }
        public int RobaID { get; set; }
        public double ProdajnaCena { get; set; }
        public double Poruceno { get; set; }
        public double Skart { get; set; }

        private static object _insertLock = new object();
        private static ManualResetEventSlim _updateMRE = new ManualResetEventSlim(true);

        public StavkaPopis()
        {

        }

        /// <summary>
        /// Azurira stavku u bazi. Osnov: ID
        /// </summary>
        public void Update()
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Update(con);
            }
        }
        /// <summary>
        /// Azurira stavku u bazi. Osnov: ID
        /// </summary>
        public void Update(FbConnection con)
        {
            using (FbCommand cmd = new FbCommand(@"UPDATE STAVKA_POPIS SET
                    KOLICINA = @KOL,
                    PORUCENO = @POR,
                    SKART = @SKAR,
                    PRODAJNA_CENA = @PC
                    WHERE STAVKA_POPIS_ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@KOL", Kolicina);
                cmd.Parameters.AddWithValue("@POR", Poruceno);
                cmd.Parameters.AddWithValue("@SKAR", Skart);
                cmd.Parameters.AddWithValue("@PC", ProdajnaCena);
                cmd.Parameters.AddWithValue("@ID", ID);

                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Vraca listu svih stavki popisa iz baze
        /// </summary>
        /// <returns></returns>
        public static List<StavkaPopis> List()
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return List(con);
            }
        }
        /// <summary>
        /// Vraca listu svih stavki popisa iz baze
        /// </summary>
        /// <returns></returns>
        public static List<StavkaPopis> List(FbConnection con)
        {
            List<StavkaPopis> list = new List<StavkaPopis>();

            using (FbCommand cmd = new FbCommand(@"
                SELECT STAVKA_POPIS_ID, KOLICINA, BRDOK, ROBAID, PORUCENO, SKART, PRODAJNA_CENA FROM STAVKA_POPIS", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new StavkaPopis()
                        {
                            ID = Convert.ToInt32(dr["STAVKA_POPIS_ID"]),
                            Kolicina = dr["KOLICINA"] is DBNull ? null : (double?)Convert.ToDouble(dr["KOLICINA"]),
                            BrDok = Convert.ToInt32(dr["BRDOK"]),
                            Skart = Convert.ToDouble(dr["SKART"]),
                            Poruceno = Convert.ToDouble(dr["PORUCENO"]),
                            ProdajnaCena = Convert.ToDouble(dr["PRODAJNA_CENA"]),
                            RobaID = Convert.ToInt32(dr["ROBAID"])
                        });
            }
            _buffer.Set(list);
            return list;
        }
        /// <summary>
        /// Vraca listu stavki za dati popis
        /// </summary>
        /// <param name="brojPopisa"></param>
        /// <returns></returns>
        public static List<StavkaPopis> ListByDokument(int brojPopisa)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return ListByDokument(con, brojPopisa);
            }
        }
        /// <summary>
        /// Vraca listu stavki za dati popis
        /// </summary>
        /// <param name="brojPopisa"></param>
        /// <returns></returns>
        public static Task<List<StavkaPopis>> ListByDokumentAsync(int brojPopisa)
        {
            return Task.Run(() =>
            {
                return ListByDokument(brojPopisa);
            });
        }
        /// <summary>
        /// Vraca listu stavki za dati popis
        /// </summary>
        /// <param name="con"></param>
        /// <param name="brojPopisa"></param>
        /// <returns></returns>
        public static List<StavkaPopis> ListByDokument(FbConnection con, int brojPopisa)
        {
            List<StavkaPopis> list = new List<StavkaPopis>();

            using (FbCommand cmd = new FbCommand(@"
                SELECT STAVKA_POPIS_ID, KOLICINA, BRDOK, ROBAID, PORUCENO, SKART, PRODAJNA_CENA FROM STAVKA_POPIS WHERE BRDOK = @BRDOK", con))
            {
                cmd.Parameters.AddWithValue("@BRDOK", brojPopisa);
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new StavkaPopis()
                        {
                            ID = Convert.ToInt32(dr["STAVKA_POPIS_ID"]),
                            Kolicina = dr["KOLICINA"] is DBNull ? null : (double?)Convert.ToDouble(dr["KOLICINA"]),
                            BrDok = Convert.ToInt32(dr["BRDOK"]),
                            Skart = Convert.ToDouble(dr["SKART"]),
                            Poruceno = Convert.ToDouble(dr["PORUCENO"]),
                            ProdajnaCena = Convert.ToDouble(dr["PRODAJNA_CENA"]),
                            RobaID = Convert.ToInt32(dr["ROBAID"])
                        });
            }
            _buffer.Set(list);
            return list;
        }
        /// <summary>
        /// Vraca listu svih stavki popisa iz buffera.
        /// Ukoliko je buffer zastareo osvezice ga i vratiti nove podatke.
        /// </summary>
        /// <returns></returns>
        public static List<StavkaPopis> BufferedList(TimeSpan outdateTimeout)
        {
            return _buffer.Get(outdateTimeout);
        }
        /// <summary>
        /// Dodaje novu stavku popisa u bazu i vraca ID novokreirane stavke
        /// </summary>
        /// <param name="brDok"></param>
        /// <param name="robaID"></param>
        /// <param name="ProdajnaCena"></param>
        /// <param name="kolicina"></param>
        /// <param name="poruceno"></param>
        /// <param name="skart"></param>
        /// <returns></returns>
        public static int Insert(int brDok, int robaID, double prodajnaCena, double kolicina, double poruceno, double skart)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Insert(con, brDok, robaID, prodajnaCena, kolicina, poruceno, skart);
            }
        }
        /// <summary>
        /// Dodaje novu stavku popisa u bazu i vraca ID novokreirane stavke
        /// </summary>
        /// <param name="brDok"></param>
        /// <param name="robaID"></param>
        /// <param name="ProdajnaCena"></param>
        /// <param name="kolicina"></param>
        /// <param name="poruceno"></param>
        /// <param name="skart"></param>
        /// <returns></returns>
        public static int Insert(FbConnection con, int brDok, int robaID, double prodajnaCena, double? kolicina, double poruceno, double skart)
        {
            lock (_insertLock)
            {
                using (FbCommand cmd = new FbCommand(@"INSERT INTO STAVKA_POPIS
                    (STAVKA_POPIS_ID, KOLICINA, ROBAID, PORUCENO, SKART, BRDOK, PRODAJNA_CENA)
                    VALUES
                    (((SELECT COALESCE(MAX(STAVKA_POPIS_ID), 0) FROM STAVKA_POPIS) + 1), 
                    @QUANTITY, @ROBAID, @PORUCENO, @SKART, @BRDOK, @PC) RETURNING STAVKA_POPIS_ID", con))
                {
                    cmd.Parameters.AddWithValue("@QUANTITY", kolicina);
                    cmd.Parameters.AddWithValue("@ROBAID", robaID);
                    cmd.Parameters.AddWithValue("@PORUCENO", poruceno);
                    cmd.Parameters.AddWithValue("@SKART", skart);
                    cmd.Parameters.AddWithValue("@BRDOK", brDok);
                    cmd.Parameters.AddWithValue("@PC", prodajnaCena);
                    cmd.Parameters.Add(new FbParameter { ParameterName = "STAVKA_POPIS_ID", FbDbType = FbDbType.Integer, Direction = System.Data.ParameterDirection.Output });
                    cmd.ExecuteNonQuery();

                    return Convert.ToInt32(cmd.Parameters["STAVKA_POPIS_ID"].Value);
                }
            }
        }
        /// <summary>
        /// Uklanja stavku popisa iz baze
        /// </summary>
        /// <param name="stavkaID"></param>
        public static void Remove(int stavkaID)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Remove(con, stavkaID);
            }
        }
        /// <summary>
        /// Uklanja stavku popisa iz baze
        /// </summary>
        /// <param name="stavkaID"></param>
        public static void Remove(FbConnection con, int stavkaID)
        {
            using (FbCommand cmd = new FbCommand("DELETE FROM STAVKA_POPIS WHERE STAVKA_POPIS_ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", stavkaID);
                cmd.ExecuteNonQuery();
            }
            _buffer.Update();
        }
        public static void RemoveAllFromDocument(int brDok)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                RemoveAllFromDocument(con, brDok);
            }
        }
        public static void RemoveAllFromDocument(FbConnection con, int brDok)
        {
            using (FbCommand cmd = new FbCommand("DELETE FROM STAVKA_POPIS WHERE BRDOK = @BRDOK", con))
            {
                cmd.Parameters.AddWithValue("@BRDOK", brDok);
                cmd.ExecuteNonQuery();
            }
            _buffer.Update();
        }
    }
}