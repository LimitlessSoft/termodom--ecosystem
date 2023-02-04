using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    /// <summary>
    /// Tabela config
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Config<T>
    {
        public int ID { get; set; }
        public T Tag { get; set; } = default(T);

        /// <summary>
        /// Azurira ili dodaje podatke u bazi. Ako je RAW true onda nece serijalizovati TAG vec azurirati takvog kakav je. Osnov: ID
        /// </summary>
        public void UpdateOrInsert(bool raw = false)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                UpdateOrInsert(con, raw);
            }
        }
        /// <summary>
        /// Azurira ili dodaje podatke u bazi. Ako je RAW true onda nece serijalizovati TAG vec azurirati takvog kakav je. Osnov: ID
        /// </summary>
        public void UpdateOrInsert(FbConnection con, bool raw = false)
        {
            using (FbCommand cmd = new FbCommand("UPDATE OR INSERT INTO CONFIG (ID, TAG) VALUES (@ID, @TAG) MATCHING(ID)", con))
            {
                cmd.Parameters.AddWithValue("@ID", ID);
                byte[] tagBytes = raw ? Encoding.UTF8.GetBytes(Tag.ToString()) : Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Tag));
                cmd.Parameters.AddWithValue("@TAG", tagBytes);

                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Vraca podatke iz baze
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Config<T> Get(int id)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                using(FbCommand cmd = new FbCommand("SELECT TAG FROM CONFIG WHERE ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            return new Config<T>()
                            {
                                ID = id,
                                Tag = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString((byte[])dr["TAG"]))
                            };
                }
            }
            
            return new Config<T>() { ID = id };
        }
        public static Config<string> GetRaw(ConfigParameter parameter)
        {
            return GetRaw((int)parameter);
        }
        /// <summary>
        /// Vraca podatke iz baze
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Config<string> GetRaw(int id)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT TAG FROM CONFIG WHERE ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            return new Config<string>()
                            {
                                ID = id,
                                Tag = Encoding.UTF8.GetString((byte[])dr["TAG"])
                            };
                }
            }

            return new Config<string>() { ID = id };
        }
        public static Config<T> Get(ConfigParameter parameter)
        {
            return Get((int)parameter);
        }
        public static Task<Config<T>> GetAsync(int id)
        {
            return Task.Run(() =>
            {
                return Get(id);
            });
        }
        public static Task<Config<T>> GetAsync(ConfigParameter parameter)
        {
            return Task.Run(() =>
            {
                return Get((int)parameter);
            });
        }
        public static List<Config<T>> List()
        {
            List<Config<T>> list = new List<Config<T>>();
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT ID, TAG FROM CONFIG", con))
                {
                    using (FbDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            list.Add(new Config<T>()
                            {
                                ID = Convert.ToInt32(dr[0]),
                                Tag = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString((byte[])dr["TAG"]))
                            });
                }
            }

            return list;
        }
        /// <summary>
        /// Vraca listu configa ali umesto tag da deserijalizuje u objekat koji treba on vraca cist JSON string
        /// </summary>
        /// <returns></returns>
        public static List<Config<string>> ListRaw()
        {
            List<Config<string>> list = new List<Config<string>>();
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT ID, TAG FROM CONFIG", con))
                {
                    using (FbDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            list.Add(new Config<string>()
                            {
                                ID = Convert.ToInt32(dr[0]),
                                Tag = Encoding.UTF8.GetString((byte[])dr["TAG"])
                            });
                }
            }

            return list;
        }
    }
}
