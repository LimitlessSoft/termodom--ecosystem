using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public class Firma
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        public string PIB { get; set; }
        public string MB { get; set; }
        public string Adresa { get; set; }
        public string TekuciRacun { get; set; }
        public string Grad { get; set; }

        public Firma()
        {

        }
        public static int Insert(string naziv, string pib, string mb, string adresa, string tekuciRacun, string grad)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Insert(con, naziv, pib, mb, adresa, tekuciRacun, grad);
            }
        }
        public static int Insert(FbConnection con, string naziv, string pib, string mb, string adresa, string tekuciRacun, string grad)
        {
            if (string.IsNullOrWhiteSpace(naziv))
                throw new Exceptions.FailedDatabaseInsertException("Morate proslediti naziv!");

            if (string.IsNullOrWhiteSpace(pib))
                throw new Exceptions.FailedDatabaseInsertException("Morate proslediti pib!");

            if (string.IsNullOrWhiteSpace(mb))
                throw new Exceptions.FailedDatabaseInsertException("Morate proslediti mb!");

            if (string.IsNullOrWhiteSpace(adresa))
                throw new Exceptions.FailedDatabaseInsertException("Morate proslediti adresu!");

            if (string.IsNullOrWhiteSpace(tekuciRacun))
                throw new Exceptions.FailedDatabaseInsertException("Morate proslediti tekuci racun!");

            if (string.IsNullOrWhiteSpace(grad))
                throw new Exceptions.FailedDatabaseInsertException("Morate proslediti grad!");

            using(FbCommand cmd = new FbCommand(@"INSERT INTO FIRMA
(ID, NAZIV, PIB, MB, ADRESA, TR, GRAD)
VALUES
(((SELECT COALESCE(MAX(ID), 0) FROM FIRMA) + 1), @NAZ, @PIB, @MB, @ADR, @TR, @GRAD) RETURNING ID", con))
            {
                cmd.Parameters.Add(new FbParameter("ID", FbDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.AddWithValue("@NAZ", naziv);
                cmd.Parameters.AddWithValue("@PIB", pib);
                cmd.Parameters.AddWithValue("@MB", mb);
                cmd.Parameters.AddWithValue("@ADR", adresa);
                cmd.Parameters.AddWithValue("@TR", tekuciRacun);
                cmd.Parameters.AddWithValue("@GRAD", grad);

                cmd.ExecuteNonQuery();

                if (cmd.Parameters["ID"].Value is DBNull)
                    throw new Exceptions.FailedDatabaseInsertException("Greska prilikom inserta u bazu!");

                return Convert.ToInt32(cmd.Parameters["ID"].Value);
            }
        }
        public static Task<int> InsertAsync(string naziv, string pib, string mb, string adresa, string tekuciRacun, string grad)
        {
            return Task.Run(() =>
            {
                return Insert(naziv, pib, mb, adresa, tekuciRacun, grad);
            });
        }
        public static Firma Get(int id)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Get(con, id);
            }
        }
        public static Firma Get(FbConnection con, int id)
        {
            using (FbCommand cmd = new FbCommand("SELECT ID, NAZIV, PIB, MB, ADRESA, TR, GRAD FROM FIRMA WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new Firma()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Naziv = dr["NAZIV"].ToString(),
                            PIB = dr["PIB"].ToString(),
                            MB = dr["MB"].ToString(),
                            Adresa = dr["ADRESA"].ToString(),
                            TekuciRacun = dr["TR"].ToString(),
                            Grad = dr["GRAD"].ToString()
                        };
            }

            return null;
        }
        public static List<Firma> List()
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return List(con);
            }
        }
        public static List<Firma> List(FbConnection con)
        {
            List<Firma> list = new List<Firma>();
            using (FbCommand cmd = new FbCommand("SELECT ID, NAZIV, PIB, MB, ADRESA, TR, GRAD FROM FIRMA", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new Firma()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Naziv = dr["NAZIV"].ToString(),
                            PIB = dr["PIB"].ToString(),
                            MB = dr["MB"].ToString(),
                            Adresa = dr["ADRESA"].ToString(),
                            TekuciRacun = dr["TR"].ToString(),
                            Grad = dr["GRAD"].ToString()
                        });
            }

            return list;
        }
        public static Task<List<Firma>> ListAsync()
        {
            return Task.Run(() =>
            {
                return List();
            });
        }
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
            using (FbCommand cmd = new FbCommand("UPDATE FIRMA SET NAZIV = @N, PIB = @P, MB = @M, ADRESA = @A, TR = @T, GRAD = @G WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@N", Naziv);
                cmd.Parameters.AddWithValue("@P", PIB);
                cmd.Parameters.AddWithValue("@M", MB);
                cmd.Parameters.AddWithValue("@A", Adresa);
                cmd.Parameters.AddWithValue("@T", TekuciRacun);
                cmd.Parameters.AddWithValue("@G", Grad);

                cmd.ExecuteNonQuery();
            }
        }
        public Task UpdateAsync()
        {
            return Task.Run(() =>
            {
                Update();
            });
        }
    }
}
