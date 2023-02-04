using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.Komercijalno
{
    public class Komentari
    {
        public int VrDok { get; set; }
        public int BrDok { get; set; }
        public string Komentar { get; set; }
        public string IntKomentar { get; set; }
        public string PrivKomentar { get; set; }

        public Komentari()
        {

        }

        public void Update()
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                Update(con);
            }
        }
        public void Update(FbConnection con)
        {
            using(FbCommand cmd = new FbCommand("UPDATE KOMENTARI SET KOMENTAR = @KOMENTAR, INTKOMENTAR = @INTKOMENTAR, PRIVKOMENTAR = @PRIVKOMENTAR WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK", con))
            {
                cmd.Parameters.AddWithValue("@KOMENTAR", Komentar);
                cmd.Parameters.AddWithValue("@INTKOMENTAR", IntKomentar);
                cmd.Parameters.AddWithValue("@PRIVKOMENTAR", PrivKomentar);
                cmd.Parameters.AddWithValue("@VRDOK", VrDok);
                cmd.Parameters.AddWithValue("@BRDOK", BrDok);

                cmd.ExecuteNonQuery();
            }
        }

        public static Komentari Get(int vrDok, int brDok)
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                return Get(con, vrDok, brDok);
            }
        }
        public static Komentari Get(FbConnection con, int vrDok, int brDok)
        {
            using (FbCommand cmd = new FbCommand("SELECT KOMENTAR, INTKOMENTAR, PRIVKOMENTAR FROM KOMENTARI WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK", con))
            {
                cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                cmd.Parameters.AddWithValue("@BRDOK", brDok);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new Komentari()
                        {
                            Komentar = dr["KOMENTAR"].ToStringOrDefault(),
                            IntKomentar = dr["INTKOMENTAR"].ToStringOrDefault(),
                            PrivKomentar = dr["PRIVKOMENTAR"].ToStringOrDefault()
                        };
            }

            return null;
        }
        public static Task<Komentari> GetAsync(int vrDok, int brDok)
        {
            return Task.Run(() =>
            {
                return Get(vrDok, brDok);
            });
        }

        public static void Insert(int vrDok, int brDok, string komentar, string intKomentar, string privKomentar)
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                Insert(con, vrDok, brDok, komentar, intKomentar, privKomentar);
            }
        }
        public static void Insert(FbConnection con, int vrDok, int brDok, string komentar, string intKomentar, string privKomentar)
        {
            using(FbCommand cmd = new FbCommand("INSERT INTO KOMENTARI (VRDOK, BRDOK, KOMENTAR, INTKOMENTAR, PRIVKOMENTAR) VALUES (@VRDOK, @BRDOK, @KOM, @INTKOM, @PRIVKOM)", con))
            {
                cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                cmd.Parameters.AddWithValue("@BRDOK", brDok);
                cmd.Parameters.AddWithValue("@KOM", komentar);
                cmd.Parameters.AddWithValue("@INTKOM", intKomentar);
                cmd.Parameters.AddWithValue("@PRIVKOM", privKomentar);
                
                cmd.ExecuteNonQuery();
            }
        }

    }
}
