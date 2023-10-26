using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Akc
    {
        public int ID { get; set; }
        public string Action { get; set; }
        public DateTime Date { get; set; }
        public int Sender { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="sender"></param>
        public static void Insert(string action, int sender)
        {
            using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO AKC (ACTION, DATE, SENDER) VALUES (@ACTION, @DATE, @SENDER)", con))
                {
                    cmd.Parameters.AddWithValue("@ACTION", action);
                    cmd.Parameters.AddWithValue("@DATE", DateTime.Now);
                    cmd.Parameters.AddWithValue("@SENDER", sender);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Akc> List()
        {
            List<Akc> list = new List<Akc>();
            using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT ID, ACTION, DATE, SENDER FROM AKC", con))
                {
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                        list.Add(new Akc() {
                            ID = Convert.ToInt32(dr["ID"]),
                            Action = dr["ACTION"].ToString(),
                            Date = Convert.ToDateTime(dr["DATE"]),
                            Sender = Convert.ToInt32(dr["SENDER"])
                        });
                }
            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public static void Delete(int id)
        {
            using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("DELETE FROM AKC WHERE ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("ID", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
