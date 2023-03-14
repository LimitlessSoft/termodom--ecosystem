using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public class SMS
    {
        public int ID { get; set; }
        public SMSStatus Status { get; set; }
        public string Broj { get; set; }
        public string Text { get; set; }

        public SMS()
        {

        }

        public void Update()
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Update(con);
            }
        }
        public void Update(FbConnection con)
        {
            using(FbCommand cmd = new FbCommand("UPDATE SMS SET STATUS = @S, TEXT = @T WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@S", (int)Status);
                cmd.Parameters.AddWithValue("@T", Text);
                cmd.Parameters.AddWithValue("@ID", ID);

                cmd.ExecuteNonQuery();
            }
        }

        public static int Insert(string broj, string text)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Insert(con, broj, text);
            }
        }
        public static int Insert(FbConnection con, string broj, string text)
        {
            using(FbCommand cmd = new FbCommand("INSERT INTO SMS (ID, STATUS, BROJ, TEXT) VALUES (((SELECT COALESCE(MAX(ID), 0) FROM SMS) + 1), 0, @BROJ, @TEXT) RETURNING ID", con))
            {
                cmd.Parameters.AddWithValue("@BROJ", broj);
                cmd.Parameters.AddWithValue("@TEXT", text);
                cmd.Parameters.Add(new FbParameter("ID", FbDbType.Integer) { Direction = System.Data.ParameterDirection.Output });

                cmd.ExecuteNonQuery();

                return Convert.ToInt32(cmd.Parameters["ID"].Value);
            }
        }

        public static SMS Get(int id)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Get(con, id);
            }
        }
        public static SMS Get(FbConnection con, int id)
        {
            using(FbCommand cmd = new FbCommand("SELECT ID, STATUS, BROJ, TEXT FROM SMS WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new SMS()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Status = (SMSStatus)Convert.ToInt32(dr["STATUS"]),
                            Broj = dr["BROJ"].ToString(),
                            Text = dr["TEXT"].ToString()
                        };
            }

            return null;
        }
        public static Task<SMS> GetAsync(int id)
        {
            return Task.Run(() =>
            {
                return Get(id);
            });
        }

        public static List<SMS> List()
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return List(con);
            }
        }
        public static List<SMS> List(FbConnection con)
        {
            List<SMS> list = new List<SMS>();
            using (FbCommand cmd = new FbCommand("SELECT ID, STATUS, BROJ, TEXT FROM SMS", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new SMS()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Status = (SMSStatus)Convert.ToInt32(dr["STATUS"]),
                            Broj = dr["BROJ"].ToString(),
                            Text = dr["TEXT"].ToString()
                        });
            }

            return list;
        }
        public static Task<List<SMS>> ListAsync()
        {
            return Task.Run(() =>
            {
                return List();
            });
        }
        
        public static void Delete(int id)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Delete(con, id);
            }
        }
        public static void Delete(FbConnection con, int id)
        {
            using (FbCommand cmd = new FbCommand("DELETE FROM SMS WHERE ID = @ID", con))
            {
                cmd.Parameters.Add("@ID", id);
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
            using(FbCommand cmd = new FbCommand("DELETE FROM SMS", con))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
