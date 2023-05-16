using System;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
    public class Modul
    {
        public int ID { get; set; }
        public string Komentar { get; set; }
        public string InterniKomentar { get; set; }

        public void Update()
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Update(con);
            }
        }
        public void Update(FbConnection con)
        {
            using (FbCommand cmd = new FbCommand(@"UPDATE MODUL SET 
                KOMENTAR = @K, INTERNI_KOMENTAR = @IK WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@K", Komentar);
                cmd.Parameters.AddWithValue("@IK", InterniKomentar);

                cmd.ExecuteNonQuery();
            }
        }

        public static Modul GetWithInsert(int id)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return GetWithInsert(con, id);
            }
        }

        public static Modul GetWithInsert(FbConnection con, int id)
        {
            using (FbCommand cmd = new FbCommand("SELECT ID, KOMENTAR, INTERNI_KOMENTAR FROM MODUL WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                using (FbDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new Modul()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Komentar = (dr["KOMENTAR"]).ToString(),
                            InterniKomentar = (dr["INTERNI_KOMENTAR"]).ToString()
                        };
                    }
                    else
                    {
                            using (FbCommand cmd1 = new FbCommand("INSERT INTO MODUL (ID, KOMENTAR, INTERNI_KOMENTAR) VALUES (@ID, @K, @IK )", con))
                        {
                            cmd1.Parameters.AddWithValue("@ID", id);
                            cmd1.Parameters.AddWithValue("@K", "");
                            cmd1.Parameters.AddWithValue("@IK", "");

                            cmd1.ExecuteNonQuery();
                        }
                        return new Modul()
                        {
                            ID = id,
                            Komentar = "",
                            InterniKomentar = ""
                        };
                    }
                    
                }
            }
        }
    }
}
