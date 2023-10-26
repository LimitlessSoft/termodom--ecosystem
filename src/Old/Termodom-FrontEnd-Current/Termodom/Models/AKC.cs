using MySql.Data.MySqlClient;
using System;

namespace Termodom.Models
{
    // TODO: Transfer to API
    /// <summary>
    /// TODO:
    /// Ovo treba biti prebaceno na API da ne komunicira direktno sa bazom
    /// </summary>
    public class AKC
    {
        public string Action { get; set; }
        public DateTime Date { get; set; }
        public int Sender { get; set; }

        public static AKC Get(string action)
        {
            using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT ACTION, DATE, SENDER FROM AKC WHERE ACTION = @q", con))
                {
                    cmd.Parameters.AddWithValue("@q", action);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            return new AKC()
                            {
                                Action = dr["ACTION"].ToString(),
                                Date = Convert.ToDateTime(dr["DATE"]),
                                Sender = Convert.ToInt32(dr["SENDER"])
                            };
                }
            }
            return null;
        }
        public static void Insert(string akcija, int posiljalac)
        {
            using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO AKC (ACTION, DATE, SENDER) VALUES (@A, @D, @U)", con))
                {
                    cmd.Parameters.AddWithValue("@A", akcija);
                    cmd.Parameters.AddWithValue("@D", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@U", posiljalac);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
