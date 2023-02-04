using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public class SMSIstorija
    {
        public int ID { get; set; }
        public int PartnerID { get; set; }
        public string Tekst { get; set; }
        public DateTime Datum { get; set; }
        /// <summary>
        /// Prilikom slanja poruka generise se hash duzine 8 karaktera koji grupise poruke koje su zadate
        /// na slanje u tom trenutku. Ako je u jednom trenutku zadato 10 poruka da se salje, svih 10 poruka ce pripadati istom
        /// setu
        /// </summary>
        public string Hash { get; set; }

        public SMSIstorija()
        {

        }

        public static List<string> SetList()
        {
            List<string> list = new List<string>();
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT DISTINCT HASH FROM SMS_ISTORIJA", con))
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(dr[0].ToString());
            }
            return list;
        }
        public static string GenerateRandomSetHash()
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(Random.Next(0, 10000000).ToString()));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static int Insert(int partnerID, string tekst, DateTime datum, string hash)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Insert(con, partnerID, tekst, datum, hash);
            }
        }
        public static int Insert(FbConnection con, int partnerID, string tekst, DateTime datum, string hash)
        {
            using(FbCommand cmd = new FbCommand(@"INSERT INTO SMS_ISTORIJA (ID, PARTNER_ID, TEKST, DATUM, HASH) VALUES
(((SELECT COALESCE(MAX(ID), 0) FROM SMS_ISTORIJA) + 1), @PID, @TEKST, @DATUM, @HASH) RETURNING ID", con))
            {
                cmd.Parameters.Add(new FbParameter("ID", FbDbType.Integer) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters.AddWithValue("@PID", partnerID);
                cmd.Parameters.AddWithValue("@TEKST", tekst);
                cmd.Parameters.AddWithValue("@DATUM", datum);
                cmd.Parameters.AddWithValue("@HASH", hash);

                cmd.ExecuteNonQuery();

                if(cmd.Parameters["ID"].Value == null)
                {
                    throw new NullReferenceException();
                }

                return Convert.ToInt32(cmd.Parameters["ID"].Value);
            }
        }
        public static Task<int> InsertAsync(int partnerID, string tekst, DateTime datum, string hash)
        {
            return Task.Run(() =>
            {
                return Insert(partnerID, tekst, datum, hash);
            });
        }

        public static SMSIstorija Get(int id)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Get(con, id);
            }
        }
        public static SMSIstorija Get(FbConnection con, int id)
        {
            using(FbCommand cmd = new FbCommand("SELECT ID, PARTNER_ID, TEKST, DATUM, HASH FROM SMS_ISTORIJA WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new SMSIstorija()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            PartnerID = Convert.ToInt32(dr["PARTNER_ID"]),
                            Tekst = dr["TEKST"].ToString(),
                            Datum = Convert.ToDateTime(dr["DATUM"]),
                            Hash = dr["HASH"].ToString()
                        };
            }

            return null;
        }
        public static Task<SMSIstorija> GetAsync(int id)
        {
            return Task.Run(() =>
            {
                return Get(id);
            });
        }

        public static List<SMSIstorija> List()
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return List(con);
            }
        }
        public static List<SMSIstorija> List(FbConnection con)
        {
            List<SMSIstorija> list = new List<SMSIstorija>();

            using (FbCommand cmd = new FbCommand("SELECT ID, PARTNER_ID, TEKST, DATUM, HASH FROM SMS_ISTORIJA", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new SMSIstorija()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            PartnerID = Convert.ToInt32(dr["PARTNER_ID"]),
                            Tekst = dr["TEKST"].ToString(),
                            Datum = Convert.ToDateTime(dr["DATUM"]),
                            Hash = dr["HASH"].ToString()
                        });
            }

            return list;
        }
        public static Task<List<SMSIstorija>> ListAsync()
        {
            return Task.Run(() =>
            {
                return List();
            });
        }

        public static void Delete(int id)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Delete(con, id);
            }
        }
        public static void Delete(FbConnection con, int id)
        {
            using(FbCommand cmd = new FbCommand("DELETE FROM SMS_ISTORIJA WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                cmd.ExecuteNonQuery();
            }
        }

        public static void Clear()
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Clear(con);
            }
        }
        public static void Clear(FbConnection con)
        {
            using (FbCommand cmd = new FbCommand("DELETE FROM SMS_ISTORIJA", con))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
