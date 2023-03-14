using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TDOffice_v2.TDOffice
{
    public class PravoDefinicija
    {
        public int ModulID { get; set; }
        public string Naziv { get; set; }
        

        public PravoDefinicija()
        {

        }

        public PravoDefinicija(int modulID, string naziv)
        {
            this.ModulID = modulID;
            this.Naziv = naziv;
        }
    }
    public class Pravo
    {
        public static List<PravoDefinicija> definicijePrava = new List<PravoDefinicija>()
        {
            new PravoDefinicija(0, "Korisnici - Modul"),
            new PravoDefinicija(1, "Whiteboard - Modul"),
            new PravoDefinicija(2, "Podesavanja - Postavi Minimalnu Verziju Programa"),
            new PravoDefinicija(3, "Podesavanja - Promenjive"),
            new PravoDefinicija(5020, "TDMagacinAPI - Azuriraj Sifarnik Robe Modul"),
            new PravoDefinicija(500001, "Kartica Robe - Modul"),

            new PravoDefinicija(700000, "7 TDPopis - Modul"),
            new PravoDefinicija(700001, "7 TDPopis - Novi"),
            new PravoDefinicija(700002, "7 TDPopis - Zakljucaj"),
            new PravoDefinicija(700003, "7 TDPopis - Otkljucaj"),
            new PravoDefinicija(700004, "7 TDPopis - Rad sa svim magacinima"),
            new PravoDefinicija(700801, "7 TDPopis - Vidi stanje na dan za stavku"),
            new PravoDefinicija(700802, "7 TDPopis - Vidi razliku za stavku"),
            new PravoDefinicija(700803, "7 TDPopis - Vidi vrednost razlike za stavku"),
            new PravoDefinicija(700804, "7 TDPopis - Moze da menja koju informaciju unosi prilikom izbora stavke"),
            new PravoDefinicija(700805, "7 TDPopis - Moze da generise novi popis na osnovu neslaganja - svih razlika"),
            new PravoDefinicija(700806, "7 TDPopis - Moze da stampa listu stavki koje se ne slazu - svih razlika"),
            new PravoDefinicija(700807, "7 TDPopis - Moze da stampa listu stavki koje se ne slazu - visak / manjak"),
            new PravoDefinicija(700808, "7 TDPopis - Moze da upravlja specijal stampom"),
            new PravoDefinicija(700809, "7 TDPopis - Moze da pokrene akciju kreiranja dokumenta zaduzenja"),
            new PravoDefinicija(700810, "7 TDPopis - Moze da toggluje prikaz detaljne analize"),
            new PravoDefinicija(700811, "7 TDPopis - Moze da zabelezi trenutno stanje dokumenta"),
            new PravoDefinicija(700812, "7 TDPopis - Moze da ukloni trenutno stanje dokumenta"),
            new PravoDefinicija(700813, "7 TDPopis - Moze da gleda zabelezeno stanje dokumenta"),

            new PravoDefinicija(130108, "1301 Poruka - Izmena vidljivosti poruke"),
            new PravoDefinicija(132500, "1325 Zamena Robe - Modul"),
            new PravoDefinicija(132501, "1325 Zamena Robe - Novi"),
            new PravoDefinicija(132502, "1325 Zamena Robe - Zakljucaj"),
            new PravoDefinicija(132503, "1325 Zamena Robe - Otkljucaj"),
            new PravoDefinicija(132504, "1325 Zamena Robe - Moze da menja trosak zamene"),
            new PravoDefinicija(132505, "1325 Zamena Robe - Rad sa vise magacina"),
            new PravoDefinicija(133200, "1332 Proracun - Modul"),
            new PravoDefinicija(133201, "1332 Proracun - Novi"),
            new PravoDefinicija(133202, "1332 Proracun - Zakljucaj"),
            new PravoDefinicija(133203, "1332 Proracun - Otkljucaj"),
            new PravoDefinicija(133204, "1332 Proracun - Rad sa svim magacinima"),
            new PravoDefinicija(133205, "1332 Proracun - Kloniranje proracuna u komercijalno"),
            new PravoDefinicija(133206, "1332 Proracun - Pretvori proracun u MP racun"),
            new PravoDefinicija(133500, "Kontrola Lagera - Modul"),

            new PravoDefinicija(133510, "Nova Roba - Modul"),
            new PravoDefinicija(133520, "Detalji Robe - Modul"),
            new PravoDefinicija(133521, "Detalji Robe - Azuriraj"),

            new PravoDefinicija(133600, "1336 Razduzenje Magacina - Modul"),
            new PravoDefinicija(133601, "1336 Razduzenje Magacina - Novi"),
            new PravoDefinicija(133602, "1336 Razduzenje Magacina - Zakljucaj"),
            new PravoDefinicija(133603, "1336 Razduzenje Magacina - Otkljucaj"),
            new PravoDefinicija(133604, "1336 Razduzenje Magacina - Rad sa svim magacinima"),
            new PravoDefinicija(133605, "1336 Razduzenje Magacina - Akcija Razduzi Magacin / Razduzenje magacina"),

            new PravoDefinicija(133610, "Specijalni Cenovnik - Modul"),

            new PravoDefinicija(133700, "Kontakti - Modul"),
            new PravoDefinicija(133701, "Kontakti - Blokiraj kontakt"),

            new PravoDefinicija(134400, "Specifikacija Novca - Modul"),
            new PravoDefinicija(134404, "Specifikacija Novca - Rad sa svim miagacinima"),
            new PravoDefinicija(134409, "Specifikacija Novca - Izmena prethodnih datuma"),
            new PravoDefinicija(134700, "Partner Pocetno / Krajnje - Modul"),
            new PravoDefinicija(134800, "Komercijalno Partner Novi - Modul"),
            new PravoDefinicija(135000, "Zakljucaj 500 - Modul"),
            new PravoDefinicija(135200, "Partner / Povezi Uplate - Modul"),
            new PravoDefinicija(136000, "Ocekivane Uplate - Modul"),

            new PravoDefinicija(136100, "Cekovi - Modul"),
            new PravoDefinicija(136101, "Cekovi - Novi"),
            new PravoDefinicija(136102, "Cekovi - Rad sa svim magacinima"),
            new PravoDefinicija(136103, "Cekovi - Stampa specifikacije"),
            new PravoDefinicija(136104, "Cekovi - Zaduzi"),
            new PravoDefinicija(136105, "Cekovi - Realizuj"),
            new PravoDefinicija(136106, "Cekovi - Ukloni /danasnji datum"),
            new PravoDefinicija(136107, "Cekovi - Ukloni /prethodni datum"),
            new PravoDefinicija(136108, "Cekovi - Novi cek /menjanje datuma ceka"),
            new PravoDefinicija(136109, "Cekovi - Izmena podataka ceka"),

            new PravoDefinicija(136500, "Sastavnica/Rastavnica Pravila - Modul"),

            new PravoDefinicija(136550, "Sastavnica/Rastavnica - Modul"),

            new PravoDefinicija(136560, "Masovni SMS - Modul"),

            new PravoDefinicija(136570, "Istorija SMS - Modul"),
            
            new PravoDefinicija(136580, "Ulazna Ponuda - Modul"),

            new PravoDefinicija(136600, "Nalog Za Prevoz - Modul"),
            new PravoDefinicija(136601, "Nalog Za Prevoz - Novi"),
            new PravoDefinicija(136602, "Nalog Za Prevoz - Zakljucaj"),
            new PravoDefinicija(136603, "Nalog Za Prevoz - Otkljucaj"),
            new PravoDefinicija(136604, "Nalog Za Prevoz - Rad sa svim magacinima"),

            new PravoDefinicija(136700, "Check Lista - Modul"),
            new PravoDefinicija(136701, "Check Lista - Vidi tudje liste"),
            new PravoDefinicija(136702, "Check Lista - Upravlja listama"),

            new PravoDefinicija(137000, "Definisanje Prodajne Cene - Modul"),
            new PravoDefinicija(137001, "Definisanje Prodajne Cene - Rad sa svim magacinima"),

            new PravoDefinicija(137010, "Definisanje Prodajne Cene Po Specijalnim Cenovnicima - Modul"),

            new PravoDefinicija(137020, "Partner Lista (Komercijalno) - Modul"),

            new PravoDefinicija(137030, "Specijalni Cenovnik - Modul"),

            new PravoDefinicija(137040, "Prodaja > Unesi partnera u dokument komercijalnog poslovanja - Modul"),
            new PravoDefinicija(137041, "Prodaja > Unesi partnera u dokument komercijalnog poslovanja - Rad sa tudjim dokumentima"),

            new PravoDefinicija(138900, "Pregled Dana - Modul"),
            new PravoDefinicija(140000, "Magacini - Modul"),

            new PravoDefinicija(168000, "Moj Kupac - Modul"),
            new PravoDefinicija(168001, "Moj Kupac - Vidi tudje"),

            new PravoDefinicija(168100, "Komercijalno Parametri - Modul"),

            new PravoDefinicija(169000, "Istorija Nabavke - Modul"),

            new PravoDefinicija(170000, "Izvestaj - Prodaja Robe"),

            new PravoDefinicija(170001, "Izvestaj - Nelogicne Marze Dokumenata"),

            new PravoDefinicija(170100, "Izvestaj - Neaktivni Partneri"),
            new PravoDefinicija(170101, "Izvestaj - Neaktivni Partneri - Rad sa svim magacinima"),

            new PravoDefinicija(170110, "Izvestaj - Partner Analiza Retro Popust"),

            new PravoDefinicija(170200, "Izvestaj - Detaljna Analiza Partnera"),

            new PravoDefinicija(171000, "Zaposleni - Modul"),

            new PravoDefinicija(1838801, "Web > Azuriraj Cene > Iron"),
            new PravoDefinicija(1838802, "Web > Azuriraj Kataloske Brojeve"),
            new PravoDefinicija(1838900, "Web > Uredjivanje Cena - Modul"),

            new PravoDefinicija(1839500, "Administacija > Komercijalno > Dokument > Oslobodi web proracun"),

            new PravoDefinicija(1845000, "Taskboard > Modul"),
            new PravoDefinicija(1845001, "Taskboard > Novi Taskboard"),
            new PravoDefinicija(1845002, "Taskboard > Task > Novi"),
            new PravoDefinicija(1845003, "Taskboard > Task > Menja Status"),
            
            new PravoDefinicija(1865000, "Planer Poruka > Modul"),

            new PravoDefinicija(1866000, "Sakrij Sve Poruke > Modul"),

            new PravoDefinicija(187000, "TD Manager > Obracun i uplata pazara")
        };

        public int ID { get; set; }
        public int UserID { get; set; }
        public int ModulID { get; set; }
        public int Value { get; set; }

        public Pravo()
        {

        }

        /// <summary>
        /// Azurira pravo u bazi. Osnov: ID
        /// </summary>
        /// <param name="pravoID"></param>
        /// <param name="status"></param>
        public void Update()
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Update(con);
            }
        }
        /// <summary>
        /// Azurira pravo u bazi. Osnov: ID
        /// </summary>
        /// <param name="pravoID"></param>
        /// <param name="status"></param>
        public void Update(FbConnection con)
        {
            using (FbCommand cmd = new FbCommand("UPDATE PRAVA_ZAP SET VREDNOST = @V WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@V", this.Value);
                cmd.Parameters.AddWithValue("@ID", this.ID);

                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Prikazuje messagebox sa porukom da korisnik nema pravo na odredjeni modul
        /// </summary>
        /// <param name="modulID"></param>
        public static void NematePravoObavestenje(int modulID)
        {
            //MessageBox.Show(string.Format("Nema pravo pristupa modulu {0}", modulID));
            using (fm_PravoPristupaModulu_Zatrazi ppm = new fm_PravoPristupaModulu_Zatrazi("Obavestenje", "Nema pravo pristupa modulu " + modulID.ToString(), modulID)) 
                ppm.ShowDialog();
            //MessageBox.Show(string.Format("Nema pravo pristupa modulu {0}", modulID));
        }
        /// <summary>
        /// Vraca pravo korisnika iz baze
        /// </summary>
        /// <param name="con"></param>
        /// <param name="pravoID"></param>
        /// <returns></returns>
        public static Pravo Get(int pravoID)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Get(con, pravoID);
            }
        }
        /// <summary>
        /// Vraca pravo korisnika iz baze
        /// </summary>
        /// <param name="con"></param>
        /// <param name="pravoID"></param>
        /// <returns></returns>
        public static Pravo Get(FbConnection con, int pravoID)
        {
            using (FbCommand cmd = new FbCommand("SELECT ID, USER_ID, MODUL_ID, VREDNOST FROM PRAVA_ZAP WHERE ID = @PID", con))
            {
                cmd.Parameters.AddWithValue("@PID", pravoID);
                using (FbDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new Pravo()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            UserID = Convert.ToInt32(dr["USER_ID"]),
                            ModulID = Convert.ToInt32(dr["MODUL_ID"]),
                            Value = Convert.ToInt32(dr["VREDNOST"])
                        };
                    }
                }
            }

            return null;
        }
        public static Pravo Get(int modulID, int userID)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Get(con, modulID, userID);
            }
        }
        public static Pravo Get(FbConnection con, int modulID, int userID)
        {
            using (FbCommand cmd = new FbCommand("SELECT ID, USER_ID, MODUL_ID, VREDNOST FROM PRAVA_ZAP WHERE USER_ID = @UID AND MODUL_ID = @MID", con))
            {
                cmd.Parameters.AddWithValue("@UID", userID);
                cmd.Parameters.AddWithValue("@MID", modulID);
                using (FbDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new Pravo()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            UserID = Convert.ToInt32(dr["USER_ID"]),
                            ModulID = Convert.ToInt32(dr["MODUL_ID"]),
                            Value = Convert.ToInt32(dr["VREDNOST"])
                        };
                    }
                }
            }

            return null;
        }
        /// <summary>
        /// Vraca listu prava korisnika iz baze
        /// </summary>
        /// <returns></returns>
        public static List<Pravo> List(string whereQuery = null)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return List(con, whereQuery);
            }
        }
        /// <summary>
        /// Vraca listu prava korisnika iz baze
        /// </summary>
        /// <returns></returns>
        public static List<Pravo> List(FbConnection con, string whereQuery = null)
        {
            if (!string.IsNullOrWhiteSpace(whereQuery))
                whereQuery = " AND " + whereQuery;

            List<Pravo> list = new List<Pravo>();
            using (FbCommand cmd = new FbCommand("SELECT ID, USER_ID, MODUL_ID, VREDNOST FROM PRAVA_ZAP WHERE 1 = 1 " + whereQuery, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Pravo()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            UserID = Convert.ToInt32(dr["USER_ID"]),
                            ModulID = Convert.ToInt32(dr["MODUL_ID"]),
                            Value = Convert.ToInt32(dr["VREDNOST"])
                        });
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// Ddodaje novo pravo u bazu
        /// </summary>
        /// <param name="modul"></param>
        /// <param name="status"></param>
        /// <param name="userID"></param>
        public static int Insert(int modul, int status, int userID)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Insert(con, modul, status, userID);
            }
        }
        /// <summary>
        /// Ddodaje novo pravo u bazu
        /// </summary>
        /// <param name="modul"></param>
        /// <param name="status"></param>
        /// <param name="userID"></param>
        public static int Insert(FbConnection con, int modul, int status, int userID)
        {
            using (FbCommand cmd = new FbCommand("INSERT INTO PRAVA_ZAP (ID, USER_ID, MODUL_ID, VREDNOST) VALUES (((SELECT COALESCE(MAX(ID), 0) FROM PRAVA_ZAP) + 1), @U, @M, @V) RETURNING ID", con))
            {
                cmd.Parameters.Add(new FbParameter("ID", FbDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.AddWithValue("@U", userID);
                cmd.Parameters.AddWithValue("@M", modul);
                cmd.Parameters.AddWithValue("@V", status);
                cmd.ExecuteNonQuery();

                return Convert.ToInt32(cmd.Parameters["ID"].Value);
            }
        }
    }
}
