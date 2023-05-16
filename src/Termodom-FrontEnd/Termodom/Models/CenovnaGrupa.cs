using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Termodom.Models
{
    /// <summary>
    /// TODO: Transfer u API
    /// 
    /// Predstavlja klasu koja se koristi za odredjivanje cenovne
    /// grupe kojoj proizvod pripada odnosno nivoa cene proizvoda.
    /// </summary>
    /// 
    ///<!--
    ///
    /// Svaki proizvod ima prikacenu jednu Cenovnu Grupu za sebe
    /// Korisnik za odredjenu cenovnu grupu (primer Cenovna Grupa ID: 1)
    /// ima definisani nivo cene 0 - 3.
    /// Cena proizvoda ima raspon OD - DO
    /// Nivo cene 0 = najskuplja cena proizvoda
    /// Nivo cene 3 = najjeftinija cena proizvoda
    /// Nivoi izmedju su rasporedjeni proporcijalno
    /// Ukoliko Proizvod pripada Cenovnoj Grupi ID 1, a korisnik za tu grupu
    /// ima dodeljen nivo 2, cena za taj proizvod zatog korisnika ce biti
    /// relativna tome
    /// -->
    public class CenovnaGrupa // TODO: CenovnaGrupaModel
    {
        public int ID { get; set; }
        public string Naziv { get; set; }

        public CenovnaGrupa()
        {

        }
        public CenovnaGrupa(int id, string naziv)
        {
            this.ID = id;
            this.Naziv = naziv;
        }

        /// <summary>
        /// Vraca odredjenu cenovnu grupu iz baze zavisno od ID-a
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CenovnaGrupa Get(int id)
        {
            using(MySqlConnection con = new MySqlConnection(Program.ConnectionString))
            {
                con.Open();
                using(MySqlCommand cmd = new MySqlCommand("SELECT CENOVNIK_GRUPAID, NAZIV FROM CENOVNIK_GRUPA WHERE ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            return new CenovnaGrupa(Convert.ToInt32(dr["CENOVNIK_GRUPAID"]), dr["NAZIV"].ToString());
                }
            }

            return null;
        }
        /// <summary>
        /// Vraca listu svih cenovnih grupa iz baze
        /// </summary>
        /// <returns></returns>
        public static List<CenovnaGrupa> List()
        {
            List<CenovnaGrupa> list = new List<CenovnaGrupa>();

            using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT CENOVNIK_GRUPAID, NAZIV FROM CENOVNIK_GRUPA", con))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            list.Add(new CenovnaGrupa(Convert.ToInt32(dr["CENOVNIK_GRUPAID"]), dr["NAZIV"].ToString()));
                }
            }

            return list;
        }
    }
}
