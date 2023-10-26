using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public partial class SpecijalniCenovnik
    {
        public int ID { get; set; }
        public string Naziv { get; set; }

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
            using(FbCommand cmd = new FbCommand("UPDATE SPECIJALNI_CENOVNIK SET NAZIV = @NAZ WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@NAZ", Naziv);
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.ExecuteNonQuery();
            }
        }
        public static SpecijalniCenovnik Get(int id)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Get(con, id);
            }
        }
        public static SpecijalniCenovnik Get(FbConnection con, int id)
        {
            using(FbCommand cmd = new FbCommand("SELECT ID, NAZIV FROM SPECIJALNI_CENOVNIK WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                using(FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new SpecijalniCenovnik() { ID = Convert.ToInt32(dr["ID"]), Naziv = dr["NAZIV"].ToString() };
            }

            return null;
        }
        public static void Insert(string naziv)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Insert(con, naziv);
            }
        }
        public static void Insert(FbConnection con, string naziv)
        {
            using(FbCommand cmd = new FbCommand("INSERT INTO SPECIJALNI_CENOVNIK (ID, NAZIV) VALUES (((SELECT COALESCE(MAX(ID), 0) FROM SPECIJALNI_CENOVNIK) + 1),@NAZIV)", con))
            {
                cmd.Parameters.AddWithValue("@NAZIV", naziv);

                cmd.ExecuteNonQuery();
            }
        }
        public static List<SpecijalniCenovnik> List()
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return List(con);
            }
        }
        public static List<SpecijalniCenovnik> List(FbConnection con)
        {
            List<SpecijalniCenovnik> list = new List<SpecijalniCenovnik>();
            using(FbCommand cmd = new FbCommand("SELECT ID, NAZIV FROM SPECIJALNI_CENOVNIK", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new SpecijalniCenovnik()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Naziv = dr["NAZIV"].ToString()
                        });
            }
            return list;
        }
    }
}
