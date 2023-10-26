using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDOffice_v2.TDMagacinAPI
{
    public class Roba
    {
        public int RobaID { get; set; }
        public string Naziv { get; set; }
        public string JM { get; set; }

        /// <summary>
        /// Insertuje datu robu u bazu
        /// </summary>
        /// <param name="con"></param>
        /// <param name="roba"></param>
        public static void Insert(List<Roba> roba, out List<int> neuspeliRobaID)
        {
            using (MySqlConnection con = new MySqlConnection(TDMagacinAPI.ConnectionString))
            {
                con.Open();
                neuspeliRobaID = new List<int>();
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO ROBA (ROBAID, NAZIV, JM) VALUES (@ROBAID, @NAZIV, @JM)", con))
                {
                    cmd.Parameters.Add("@ROBAID", MySqlDbType.Int32);
                    cmd.Parameters.Add("@NAZIV", MySqlDbType.VarChar);
                    cmd.Parameters.Add("@JM", MySqlDbType.VarChar);

                    foreach (Roba r in roba)
                    {
                        try
                        {
                            cmd.Parameters["@ROBAID"].Value = r.RobaID;
                            cmd.Parameters["@NAZIV"].Value = r.Naziv;
                            cmd.Parameters["@JM"].Value = r.JM;

                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {
                            neuspeliRobaID.Add(r.RobaID);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Insertuje datu robu u bazu
        /// </summary>
        /// <param name="con"></param>
        /// <param name="roba"></param>
        public static void Insert(MySqlConnection con, List<Roba> roba, out List<int> neuspeliRobaID)
        {
            neuspeliRobaID = new List<int>();
            using(MySqlCommand cmd = new MySqlCommand("INSERT INTO ROBA (ROBAID, NAZIV, JM) VALUES (@ROBAID, @NAZIV, @JM)", con))
            {
                cmd.Parameters.Add("@ROBAID", MySqlDbType.Int32);
                cmd.Parameters.Add("@NAZIV", MySqlDbType.VarChar);
                cmd.Parameters.Add("@JM", MySqlDbType.VarChar);

                foreach (Roba r in roba)
                {
                    try
                    {
                        cmd.Parameters["@ROBAID"].Value = r.RobaID;
                        cmd.Parameters["@NAZIV"].Value = r.Naziv;
                        cmd.Parameters["@JM"].Value = r.JM;

                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        neuspeliRobaID.Add(r.RobaID);
                    }
                }
            }
        }
        /// <summary>
        /// Brise svu robu iz tabele ROBA
        /// </summary>
        public static void Clear()
        {
            using(MySqlConnection con = new MySqlConnection(TDMagacinAPI.ConnectionString))
            {
                con.Open();
                Clear(con);
            }
        }
        /// <summary>
        /// Brise svu robu iz tabele ROBA
        /// </summary>
        public static void Clear(MySqlConnection con)
        {
            using(MySqlCommand cmd = new MySqlCommand("DELETE FROM ROBA", con))
            {
                cmd.ExecuteReader();
            }
        }
    }
}
