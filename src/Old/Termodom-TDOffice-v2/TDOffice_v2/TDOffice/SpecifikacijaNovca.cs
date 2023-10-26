using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDOffice_v2.TDOffice
{
    public partial class SpecifikacijaNovca
    {
        public class KursBlok
        {
            public double Kolicina { get; set; }
            public double Kurs { get; set; }

            public double Vrednost()
            {
                return Kolicina * Kurs;
            }
        }

        public int ID { get; set; }
        public DateTime Datum { get; set; }
        public int MagacinID { get; set; }
        public Detalji Tag { get; set; }

        public SpecifikacijaNovca()
        {

        }

        /// <summary>
        /// Azurira podatke u bazi po osnovu ID
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
        /// Azurira podatke u bazi po osnovu ID
        /// </summary>
        public void Update(FbConnection con)
        {
            // Update SPECIFIKACIJA_NOVCA SET DATUM, MAGACIN, TAG
            if (ID == 0)
                throw new Exception("ID is not valid!");

                using (FbCommand cmd = new FbCommand("UPDATE SPECIFIKACIJA_NOVCA SET TAG = @T, MAGACINID = @M, DATUM =@D WHERE ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@D", Datum);
                    cmd.Parameters.AddWithValue("@M", MagacinID);
                    cmd.Parameters.AddWithValue("@T", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Tag)));

                    cmd.ExecuteNonQuery();
                }
            
            //throw new NotImplementedException();
        }

        

        /// <summary>
        /// Vraca zbir konacni zbir specifikacije
        /// </summary>
        /// <returns></returns>
        public double Sum()
        {
            return Tag.Zbir();
        }

        /// <summary>
        /// Vraca specifikaciju novca po ID-u. Ako ne pronadje, vraca NULL
        /// </summary>
        /// <param name="specifikacijaID"></param>
        /// <returns></returns>
        public static SpecifikacijaNovca Get(int specifikacijaID)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT ID, DATUM, MAGACINID, TAG FROM SPECIFIKACIJA_NOVCA WHERE ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@ID", specifikacijaID);

                    using (FbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                            return new SpecifikacijaNovca()
                            {
                                ID = Convert.ToInt32(dr["ID"]),
                                MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                                Datum = Convert.ToDateTime(dr["DATUM"]),
                                Tag = JsonConvert.DeserializeObject<Detalji>(Encoding.UTF8.GetString((byte[])dr["TAG"]))
                            };
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Vraca specifikaciju novca po magainu za dati datum. Ako ne pronadje vraca NULL
        /// </summary>
        /// <param name="magacinID"></param>
        /// <param name="datum"></param>
        /// <returns></returns>
        public static SpecifikacijaNovca Get(int magacinID, DateTime datum)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Get(con, magacinID, datum);
            }
        }
        public static SpecifikacijaNovca Get(FbConnection con, int magacinID, DateTime datum)
        {
            using (FbCommand cmd = new FbCommand("SELECT ID, DATUM, MAGACINID, TAG FROM SPECIFIKACIJA_NOVCA WHERE DATUM = @D AND MAGACINID = @M", con))
            {
                cmd.Parameters.AddWithValue("@D", new DateTime(datum.Year, datum.Month, datum.Day));
                cmd.Parameters.AddWithValue("@M", magacinID);

                using (FbDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                        return new SpecifikacijaNovca()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            Datum = Convert.ToDateTime(dr["DATUM"]),
                            Tag = JsonConvert.DeserializeObject<Detalji>(Encoding.UTF8.GetString((byte[])dr["TAG"]))
                        };
                }
            }
            return null;
        }
        /// <summary>
        /// Kreira specifikaciju novca na danasnji dan
        /// </summary>
        /// <returns>ID novokreirane specifikacije</returns>
        public static int Insert(int magacinID, Detalji detalji)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Insert(con, magacinID, detalji);
            }
        }
        public static int Insert(FbConnection con, int magacinID, Detalji detalji)
        {
            using (FbCommand cmd = new FbCommand("INSERT INTO SPECIFIKACIJA_NOVCA (ID, DATUM, MAGACINID, TAG) VALUES (((SELECT COALESCE(MAX(ID), 0) FROM SPECIFIKACIJA_NOVCA) + 1), @D, @M, @T) RETURNING ID", con))
            {
                cmd.Parameters.AddWithValue("@D", new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                cmd.Parameters.AddWithValue("@M", magacinID);
                cmd.Parameters.AddWithValue("@T", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(detalji)));
                cmd.Parameters.Add(new FbParameter { ParameterName = "ID", FbDbType = FbDbType.Integer, Direction = System.Data.ParameterDirection.Output });
                cmd.ExecuteNonQuery();

                return cmd.Parameters["ID"] == null ? -1 : Convert.ToInt32(cmd.Parameters["ID"].Value);
            }
        }
        /// <summary>
        /// Kreira specifikaciju novca na odredjeni datum
        /// </summary>
        /// <returns>ID novokreirane specifikacije</returns>
        public static int Insert(int magacinID, Detalji detalji, DateTime datum)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Insert(con, magacinID, detalji, datum);
            }
        }
        public static int Insert(FbConnection con, int magacinID, Detalji detalji, DateTime datum)
        {
            using (FbCommand cmd = new FbCommand("INSERT INTO SPECIFIKACIJA_NOVCA (ID, DATUM, MAGACINID, TAG) VALUES (((SELECT COALESCE(MAX(ID), 0) FROM SPECIFIKACIJA_NOVCA) + 1), @D, @M, @T) RETURNING ID", con))
            {
                cmd.Parameters.AddWithValue("@D", new DateTime(datum.Year, datum.Month, datum.Day));
                cmd.Parameters.AddWithValue("@M", magacinID);
                cmd.Parameters.AddWithValue("@T", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(detalji)));
                cmd.Parameters.Add(new FbParameter { ParameterName = "ID", FbDbType = FbDbType.Integer, Direction = System.Data.ParameterDirection.Output });
                cmd.ExecuteNonQuery();

                return cmd.Parameters["ID"] == null ? -1 : Convert.ToInt32(cmd.Parameters["ID"].Value);
            }
        }
    }
}
