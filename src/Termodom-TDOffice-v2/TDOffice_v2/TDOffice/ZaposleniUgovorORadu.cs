using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public class ZaposleniUgovorORadu
    {
        public int ID { get; set; }
        public int ZaposleniID { get; set; }    
        public int Firma { get; set; }  
        public DateTime PocetakTrajanja { get; set; }
        public DateTime KrajTrajanja { get; set; }  

        public ZaposleniUgovorORadu()
        {

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
            using (FbCommand cmd = new FbCommand("UPDATE ZAPOSLENI_UGOVOR_O_RADU SET ZAPOSLENI_ID = @ZI, FIRMA = @F, POCETAK_TRAJANJA = @PT, KRAJ_TRAJANJA = @KT WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@F", Firma);
                cmd.Parameters.AddWithValue("@ZI", ZaposleniID);
                cmd.Parameters.AddWithValue("@PT", PocetakTrajanja);
                cmd.Parameters.AddWithValue("@KT", KrajTrajanja);

                cmd.ExecuteNonQuery();
            }
        }
        public Task UpdateAsync()
        {
            throw new NotImplementedException();
        }
        public static int Insert(int firma, int zaposleniID,DateTime pocetaktrajanja, DateTime krajtrajanja)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Insert(con, firma, zaposleniID, pocetaktrajanja, krajtrajanja);
            }
        }
        public static int Insert(FbConnection con, int firma, int zaposleniID, DateTime pocetaktrajanja, DateTime krajtrajanja)
        {
            using (FbCommand cmd = new FbCommand("INSERT INTO ZAPOSLENI_UGOVOR_O_RADU (ID, FIRMA, POCETAK_TRAJANJA, KRAJ_TRAJANJA, ZAPOSLENI_ID) VALUES (((SELECT COALESCE(MAX(ID), 0) from ZAPOSLENI_UGOVOR_O_RADU) + 1), @F, @PT, @KT, @ZI) RETURNING ID", con))
            {
                cmd.Parameters.Add("ID", FbDbType.Integer).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@F", firma);
                cmd.Parameters.AddWithValue("@PT", pocetaktrajanja);
                cmd.Parameters.AddWithValue("@KT", krajtrajanja);
                cmd.Parameters.AddWithValue("@ZI", zaposleniID);

                cmd.ExecuteNonQuery();
                return Convert.ToInt32(cmd.Parameters["ID"].Value);
            }
        }
        public static Task<int> InsertAsync(int firma, int zaposleniID, DateTime pocetaktrajanja, DateTime krajtrajanja)
        {
            return Task.Run(() =>
            {
                return Insert(firma, zaposleniID, pocetaktrajanja, krajtrajanja);
            });
        }
        public static ZaposleniUgovorORadu Get(int id)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Get(con, id);
            }
        }
        public static ZaposleniUgovorORadu Get(FbConnection con, int id)
        {
            using (FbCommand cmd = new FbCommand("SELECT ID, ZAPOSLENI_ID, FIRMA, POCETAK_TRAJANJA, KRAJ_TRAJANJA FROM ZAPOSLENI_UGOVOR_O_RADU WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new ZaposleniUgovorORadu()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            ZaposleniID = Convert.ToInt32((dr["ZAPOSLENI_ID"])),
                            KrajTrajanja = Convert.ToDateTime(dr["KRAJ_TRAJANJA"]),
                            PocetakTrajanja = Convert.ToDateTime(dr["POCETAK_TRAJANJA"]),
                            Firma = Convert.ToInt32(dr["FIRMA"])
                        };
            }

            return null;
        }
        public static Task<ZaposleniUgovorORadu> GetAsync(int id)
        {
            return Task.Run(() =>
            {
                return Get(id);
            });
        }
        public static List<ZaposleniUgovorORadu> List(string whereQuery = null)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return List(con, whereQuery);
            }
        }
        public static List<ZaposleniUgovorORadu> List(FbConnection con, string whereQuery = null)
        {
            if (!string.IsNullOrWhiteSpace(whereQuery))
                whereQuery = " AND " + whereQuery;
            List<ZaposleniUgovorORadu> list = new List<ZaposleniUgovorORadu>();
            using (FbCommand cmd = new FbCommand("SELECT ID, ZAPOSLENI_ID, FIRMA, POCETAK_TRAJANJA, KRAJ_TRAJANJA FROM ZAPOSLENI_UGOVOR_O_RADU WHERE 1 = 1" + whereQuery, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new ZaposleniUgovorORadu()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            ZaposleniID = Convert.ToInt32((dr["ZAPOSLENI_ID"])),
                            KrajTrajanja = Convert.ToDateTime(dr["KRAJ_TRAJANJA"]),
                            PocetakTrajanja = Convert.ToDateTime(dr["POCETAK_TRAJANJA"]),
                            Firma = Convert.ToInt32(dr["FIRMA"])
                        });
            }
            return list;
        }
        public static Task<List<ZaposleniUgovorORadu>> ListAsync(string whereQuery = null)
        {
            return Task.Run(() =>
            {
                return List(whereQuery);
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
            using (FbCommand cmd = new FbCommand("DELETE FROM ZAPOSLENI_UGOVOR_O_RADU WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                cmd.ExecuteNonQuery();
            }
        }
        public static Task DeleteAsync(int id)
        {
            return Task.Run(() =>
            {
                Delete(id);
            });
        }
    }
}
