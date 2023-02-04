using FirebirdSql.Data.FirebirdClient;

namespace TDBrain_v3.DB.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    public class Partner
    {
        /// <summary>
        /// Unikatan identifikator
        /// </summary>
        public int PPID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Naziv { get; set; } = "UNDEFINED";
        /// <summary>
        /// 
        /// </summary>
        public string? Adresa { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        public string? Posta { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        public string? Mesto { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        public string? Telefon { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        public string? Fax { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        public string? Email { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        public string? Kontakt { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        public double Popust { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int16 VaziDana { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int16 NeplFakt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Duguje { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Potrazuje { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? VPCID { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        public double? PROCPC { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        public Int64? Kategorija { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        public string? MBroj { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        public string? SDEL { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        public int? NPPID { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        public string? MestoID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int16? ImaUgovor { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        public Int16? ImaIzjave { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        public Int16? CenovnikID { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public int OpstinaID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DrzavaID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ZapID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Valuta { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Mobilni { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float DozvoljeniMinus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? PIB { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int VrstaCenovnika { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RefID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DrzavljanstvoID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ZanimanjeID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int WEB_Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int GPPID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Cene_od_grupe { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PDVO { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? NazivZaStampu { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? KatNaziv { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int KatID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Aktivan { get; set; }

        /// <summary>
        /// Insertuje novi red partnera u tabelu PARTNER u svim bazama
        /// </summary>
        /// <param name="naziv"></param>
        /// <param name="adresa"></param>
        /// <param name="posta"></param>
        /// <param name="telefon"></param>
        /// <param name="mobilni"></param>
        /// <param name="fax"></param>
        /// <param name="email"></param>
        /// <param name="kontakt"></param>
        /// <param name="pib"></param>
        /// <param name="kategorija"></param>
        /// <param name="aktivan"></param>
        /// <param name="mestoID"></param>
        /// <param name="mbroj"></param>
        /// <param name="opstinaID"></param>
        /// <param name="drzavaID"></param>
        /// <param name="refID"></param>
        /// <param name="pdvo"></param>
        /// <param name="nazivZaStampu"></param>
        /// <param name="valuta"></param>
        /// <param name="dozvoljeniMinus"></param>
        /// <param name="imaUgovor"></param>
        /// <param name="vrstaCenovnika"></param>
        /// <param name="drzavljanstvoID"></param>
        /// <param name="zanimanjeID"></param>
        /// <param name="webStatus"></param>
        /// <param name="gppID"></param>
        /// <param name="ceneOdGrupe"></param>
        /// <param name="vpcID"></param>
        /// <param name="procpc"></param>
        /// <param name="zapID"></param>
        /// <returns></returns>
        public static int Insert(string naziv, string adresa, string posta, string? telefon, string mobilni,
            string? fax, string email, string kontakt, string pib, Int64? kategorija, int aktivan, string mestoID,
            string mbroj, int? opstinaID, int drzavaID, int refID, int pdvo, string nazivZaStampu, string valuta,
            double dozvoljeniMinus, int imaUgovor, int vrstaCenovnika, int drzavljanstvoID, int zanimanjeID,
            int webStatus, int gppID, int ceneOdGrupe, int vpcID, double procpc, int zapID)
        {
            int noviID = -1;

            int maxID = -1;

            #region proverava da li ima konflikta id-eva
            foreach (string connString in DB.Settings.ConnectionStringKomercijalno.GetConnectionStringsDistinct(DateTime.Now.Year - 1))
            {
                using (FbConnection con = new FbConnection(connString))
                {
                    con.Open();
                    using (FbCommand cmd = new FbCommand("SELECT MAX(PPID) FROM PARTNER WHERE PPID < 100000", con))
                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            if(maxID != -1 && Convert.ToInt32(dr[0]) != maxID)
                                throw new Exceptions.PartnerInsertKonfliktIDevaException($"Maksimalni ID partnera (prvi manji od 100000) nije jedank u svim bazama! {connString}");
                            else
                                maxID = Convert.ToInt32(dr[0]);
                        else
                            throw new Exceptions.PartnerInsertKonfliktIDevaException($"Maksimalni ID partnera (prvi manji od 100000) nije jedank u svim bazama! {connString}");
                }
            }

            foreach (string connString in DB.Settings.ConnectionStringKomercijalno.GetConnectionStringsDistinct(DateTime.Now.Year))
            {
                using (FbConnection con = new FbConnection(connString))
                {
                    con.Open();
                    using (FbCommand cmd = new FbCommand("SELECT MAX(PPID) FROM PARTNER WHERE PPID < 100000", con))
                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            if (maxID != -1 && Convert.ToInt32(dr[0]) != maxID)
                                throw new Exceptions.PartnerInsertKonfliktIDevaException($"Maksimalni ID partnera (prvi manji od 100000) nije jedank u svim bazama! {connString}");
                            else
                                maxID = Convert.ToInt32(dr[0]);
                        else
                            throw new Exceptions.PartnerInsertKonfliktIDevaException($"Maksimalni ID partnera (prvi manji od 100000) nije jedank u svim bazama! {connString}");
                }
            }
            #endregion
            foreach (string connString in DB.Settings.ConnectionStringKomercijalno.GetConnectionStringsDistinct(DateTime.Now.Year - 1))
            {
                using (FbConnection con = new FbConnection(connString))
                {
                    con.Open();
                    int n = Insert(con, naziv, adresa, posta, telefon, mobilni, fax, email, kontakt, pib, kategorija, aktivan, mestoID, mbroj, opstinaID, drzavaID,
                        refID, pdvo, nazivZaStampu, valuta, dozvoljeniMinus, imaUgovor, vrstaCenovnika, drzavljanstvoID, zanimanjeID,
                        webStatus, gppID, ceneOdGrupe, vpcID, procpc, zapID);

                    if (noviID != -1 && n != noviID)
                        throw new Exception("Insertovan je novi partner u nekoliko baza ali je doslo do konflikta ID-eva! Ispratite rucno sta se desilo!");

                    noviID = n;
                }
            }

            foreach (string connString in DB.Settings.ConnectionStringKomercijalno.GetConnectionStringsDistinct(DateTime.Now.Year))
            {
                using (FbConnection con = new FbConnection(connString))
                {
                    con.Open();
                    int n = Insert(con, naziv, adresa, posta, telefon, mobilni, fax, email, kontakt, pib, kategorija, aktivan, mestoID, mbroj, opstinaID, drzavaID,
                        refID, pdvo, nazivZaStampu, valuta, dozvoljeniMinus, imaUgovor, vrstaCenovnika, drzavljanstvoID, zanimanjeID,
                        webStatus, gppID, ceneOdGrupe, vpcID, procpc, zapID);

                    if (noviID != -1 && n != noviID)
                        throw new Exception("Insertovan je novi partner u nekoliko baza ali je doslo do konflikta ID-eva! Ispratite rucno sta se desilo!");

                    noviID = n;
                }
            }

            if (noviID == -1)
                throw new Exception("Doslo je do greske prilikom insertovanja partnera u bazu!");

            return noviID;
        }
        /// <summary>
        /// Insertuje novi red partnera u tabelu PARTNER
        /// </summary>
        /// <param name="con"></param>
        /// <param name="naziv"></param>
        /// <param name="adresa"></param>
        /// <param name="posta"></param>
        /// <param name="telefon"></param>
        /// <param name="mobilni"></param>
        /// <param name="fax"></param>
        /// <param name="email"></param>
        /// <param name="kontakt"></param>
        /// <param name="pib"></param>
        /// <param name="kategorija"></param>
        /// <param name="aktivan"></param>
        /// <param name="mestoID"></param>
        /// <param name="mbroj"></param>
        /// <param name="opstinaID"></param>
        /// <param name="drzavaID"></param>
        /// <param name="refID"></param>
        /// <param name="pdvo"></param>
        /// <param name="nazivZaStampu"></param>
        /// <param name="valuta"></param>
        /// <param name="dozvoljeniMinus"></param>
        /// <param name="imaUgovor"></param>
        /// <param name="vrstaCenovnika"></param>
        /// <param name="drzavljanstvoID"></param>
        /// <param name="zanimanjeID"></param>
        /// <param name="webStatus"></param>
        /// <param name="gppID"></param>
        /// <param name="ceneOdGrupe"></param>
        /// <param name="vpcID"></param>
        /// <param name="procpc"></param>
        /// <returns>Novi PPID nakon insertovanja. Ukoliko nije uspeo da insertuje, vratice -1</returns>
        public static int Insert(FbConnection con, string naziv, string adresa, string posta, string? telefon, string mobilni,
            string? fax, string email, string kontakt, string pib, Int64? kategorija, int aktivan, string mestoID,
            string mbroj, int? opstinaID, int drzavaID, int refID, int pdvo, string nazivZaStampu, string valuta,
            double dozvoljeniMinus, int imaUgovor, int vrstaCenovnika, int drzavljanstvoID, int zanimanjeID,
            int webStatus, int gppID, int ceneOdGrupe, int vpcID, double procpc, int zapID)
        {
            using (FbCommand cmd = new FbCommand(@"INSERT INTO PARTNER 
            (PPID, NAZIV, ADRESA, POSTA, TELEFON, MOBILNI, FAX, EMAIL, KONTAKT, PIB, KATEGORIJA,
            AKTIVAN, MESTOID, MBROJ, OPSTINAID, DRZAVAID, REFID, PDVO, NPPID, NAZIVZASTAMPU, VALUTA, DOZVOLJENIMINUS,
            IMAUGOVOR, VRSTACENOVNIKA, DRZAVLJANSTVOID, ZANIMANJEID, WEB_STATUS, GPPID, CENE_OD_GRUPE, VPCID, PROCPC, ZAPID) 
            VALUES 
            (((SELECT MAX(PPID) FROM PARTNER WHERE PPID < 100000) + 1), @NAZIV, @ADRESA, @POSTA, @TELEFON, @MOBILNI, @FAX, @EMAIL, @KONTAKT, @PIB, @KATEGORIJA, @AKTIVAN,
            @MESTOID, @MBROJ, @OPSTINAID, @DRZAVAID, @REFID, @PDVO, ((SELECT MAX(PPID) FROM PARTNER WHERE PPID < 100000) + 1), @NAZIVZASTAMPU, @VALUTA, @DOZVOLJENIMINUS,
            @IMAUGOVOR, @VRSTACENOVNIKA, @DRZAVLJANSTVOID, @ZANIMANJEID, @WEB_STATUS, @GPPID, @CENE_OD_GRUPE, @VPCID, @PROCPC, @ZAPID) RETURNING PPID", con))
            {
                cmd.Parameters.Add(new FbParameter("PPID", FbDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.AddWithValue("@NAZIV", naziv);
                cmd.Parameters.AddWithValue("@ADRESA", adresa);
                cmd.Parameters.AddWithValue("@POSTA", posta);
                cmd.Parameters.AddWithValue("@TELEFON", telefon);
                cmd.Parameters.AddWithValue("@MOBILNI", mobilni);
                cmd.Parameters.AddWithValue("@FAX", fax);
                cmd.Parameters.AddWithValue("@EMAIL", email);
                cmd.Parameters.AddWithValue("@KONTAKT", kontakt);
                cmd.Parameters.AddWithValue("@PIB", pib);
                cmd.Parameters.AddWithValue("@KATEGORIJA", kategorija);
                cmd.Parameters.AddWithValue("@AKTIVAN", aktivan);
                cmd.Parameters.AddWithValue("@MESTOID", mestoID);
                cmd.Parameters.AddWithValue("@MBROJ", mbroj);
                cmd.Parameters.AddWithValue("@OPSTINAID", opstinaID);
                cmd.Parameters.AddWithValue("@DRZAVAID", drzavaID);
                cmd.Parameters.AddWithValue("@REFID", refID);
                cmd.Parameters.AddWithValue("@PDVO", pdvo);
                cmd.Parameters.AddWithValue("@NAZIVZASTAMPU", nazivZaStampu);
                cmd.Parameters.AddWithValue("@VALUTA", valuta);
                cmd.Parameters.AddWithValue("@DOZVOLJENIMINUS", dozvoljeniMinus);
                cmd.Parameters.AddWithValue("@IMAUGOVOR", imaUgovor);
                cmd.Parameters.AddWithValue("@VRSTACENOVNIKA", vrstaCenovnika);
                cmd.Parameters.AddWithValue("@DRZAVLJANSTVOID", drzavljanstvoID);
                cmd.Parameters.AddWithValue("@ZANIMANJEID", zanimanjeID);
                cmd.Parameters.AddWithValue("@WEB_STATUS", webStatus);
                cmd.Parameters.AddWithValue("@GPPID", gppID);
                cmd.Parameters.AddWithValue("@CENE_OD_GRUPE", ceneOdGrupe);
                cmd.Parameters.AddWithValue("@VPCID", vpcID);
                cmd.Parameters.AddWithValue("@PROCPC", procpc);
                cmd.Parameters.AddWithValue("@ZAPID", zapID);

                cmd.ExecuteNonQuery();

                return Convert.ToInt32(cmd.Parameters["PPID"].Value);
            }
        }

        /// <summary>
        /// Vraca objekat partnera iz baze po zadatom identifikatoru iz baze zadate godine.
        /// </summary>
        /// <param name="godina"></param>
        /// <param name="ppid"></param>
        /// <returns>Objekat partnera ili null</returns>
        public static Partner? Get(int godina, int ppid)
        {
            using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[50, godina]))
            {
                con.Open();
                return Get(con, ppid);
            }
        }
        /// <summary>
        /// Vraca objekat partnera iz baze po zadatom identifikatoru
        /// </summary>
        /// <param name="con"></param>
        /// <param name="ppid"></param>
        /// <returns>Objekat partnera ili null</returns>
        public static Partner? Get(FbConnection con, int ppid)
        {
            using (FbCommand cmd = new FbCommand(@"SELECT PPID, NAZIV, ADRESA, POSTA, MESTO, TELEFON, FAX,
                EMAIL, KONTAKT, PIB, KATEGORIJA, MBROJ, MOBILNI, MESTOID, AKTIVAN, PDVO,
                DRZAVAID, DUGUJE, POTRAZUJE FROM PARTNER WHERE PPID = @PPID", con))
            {
                cmd.Parameters.AddWithValue("@PPID", ppid);
                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
#pragma warning disable CS8601 // Possible null reference assignment.
                        return new Partner()
                        {
                            PPID = Convert.ToInt32(dr["PPID"]),
                            Naziv = Convert.ToString(dr["NAZIV"]),
                            Adresa = dr["ADRESA"] is DBNull ? null : Convert.ToString(dr["ADRESA"]),
                            Posta = dr["POSTA"] is DBNull ? null : Convert.ToString(dr["POSTA"]),
                            MestoID = dr["MESTOID"] is DBNull ? null : Convert.ToString(dr["MESTOID"]),
                            Telefon = dr["TELEFON"] is DBNull ? null : Convert.ToString(dr["TELEFON"]),
                            Fax = dr["FAX"] is DBNull ? null : Convert.ToString(dr["FAX"]),
                            Email = dr["EMAIL"] is DBNull ? null : Convert.ToString(dr["EMAIL"]),
                            Kontakt = dr["KONTAKT"] is DBNull ? null : Convert.ToString(dr["KONTAKT"]),
                            PIB = dr["PIB"] is DBNull ? null : Convert.ToString(dr["PIB"]),
                            Kategorija = dr["KATEGORIJA"] is DBNull ? (Int64?)null : Convert.ToInt64(dr["KATEGORIJA"]),
                            MBroj = dr["MBROJ"] is DBNull ? null : Convert.ToString(dr["MBROJ"]),
                            Mobilni = dr["MOBILNI"] is DBNull ? null : Convert.ToString(dr["MOBILNI"]),
                            Aktivan = Convert.ToInt32(dr["AKTIVAN"]),
                            PDVO = Convert.ToInt32(dr["PDVO"]),
                            DrzavaID = dr["DRZAVAID"] is DBNull ? 0 : Convert.ToInt32(dr["DRZAVAID"]),
                            Duguje = Convert.ToDouble(dr["DUGUJE"]),
                            Potrazuje = Convert.ToDouble(dr["POTRAZUJE"]),
                        };
#pragma warning restore CS8601 // Possible null reference assignment.
            }

            return null;
        }
        /// <summary>
        /// Vraca objekat partnera iz baze po zadatom identifikatoru iz baze zadate godine.
        /// </summary>
        /// <param name="godina"></param>
        /// <param name="pib"></param>
        /// <returns>Objekat partnera ili null</returns>
        public static Partner? Get(int godina, string pib)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[50, godina]))
            {
                con.Open();
                return Get(con, pib);
            }
        }
        /// <summary>
        /// Vraca objekat partnera iz baze po zadatom identifikatoru
        /// </summary>
        /// <param name="con"></param>
        /// <param name="pib"></param>
        /// <returns>Objekat partnera ili null</returns>
        public static Partner? Get(FbConnection con, string pib)
        {
            using (FbCommand cmd = new FbCommand(@"SELECT PPID, NAZIV, ADRESA, POSTA, MESTO, TELEFON, FAX,
                EMAIL, KONTAKT, PIB, KATEGORIJA, MBROJ, MOBILNI, MESTOID, AKTIVAN, PDVO,
                DRZAVAID, DUGUJE, POTRAZUJE FROM PARTNER WHERE PIB = @PIB", con))
            {
                cmd.Parameters.AddWithValue("@PIB", pib);
                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
#pragma warning disable CS8601 // Possible null reference assignment.
                        return new Partner()
                        {
                            PPID = Convert.ToInt32(dr["PPID"]),
                            Naziv = Convert.ToString(dr["NAZIV"]),
                            Adresa = dr["ADRESA"] is DBNull ? null : Convert.ToString(dr["ADRESA"]),
                            Posta = dr["POSTA"] is DBNull ? null : Convert.ToString(dr["POSTA"]),
                            MestoID = dr["MESTOID"] is DBNull ? null : Convert.ToString(dr["MESTOID"]),
                            Telefon = dr["TELEFON"] is DBNull ? null : Convert.ToString(dr["TELEFON"]),
                            Fax = dr["FAX"] is DBNull ? null : Convert.ToString(dr["FAX"]),
                            Email = dr["EMAIL"] is DBNull ? null : Convert.ToString(dr["EMAIL"]),
                            Kontakt = dr["KONTAKT"] is DBNull ? null : Convert.ToString(dr["KONTAKT"]),
                            PIB = dr["PIB"] is DBNull ? null : Convert.ToString(dr["PIB"]),
                            Kategorija = dr["KATEGORIJA"] is DBNull ? (Int64?)null : Convert.ToInt64(dr["KATEGORIJA"]),
                            MBroj = dr["MBROJ"] is DBNull ? null : Convert.ToString(dr["MBROJ"]),
                            Mobilni = dr["MOBILNI"] is DBNull ? null : Convert.ToString(dr["MOBILNI"]),
                            Aktivan = Convert.ToInt32(dr["AKTIVAN"]),
                            PDVO = Convert.ToInt32(dr["PDVO"]),
                            DrzavaID = dr["DRZAVAID"] is DBNull ? 0 : Convert.ToInt32(dr["DRZAVAID"]),
                            Duguje = Convert.ToDouble(dr["DUGUJE"]),
                            Potrazuje = Convert.ToDouble(dr["POTRAZUJE"]),
                        };
#pragma warning restore CS8601 // Possible null reference assignment.
            }

            return null;
        }
        /// <summary>
        /// Vraca listu partnera iz baze u vidu Dictionary-ja gde je key = PPID, a value je objekat.
        /// </summary>
        /// <param name="magacinID"></param>
        /// <param name="godina"></param>
        /// <returns></returns>
        public static Dictionary<int, Partner> Dict(int magacinID, int godina)
        {
            using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[magacinID, godina]))
            {
                con.Open();
                return Dict(con);
            }
        }
        /// <summary>
        /// Vraca listu partnera iz baze u vidu Dictionary-ja gde je key = PPID, a value je objekat.
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static Dictionary<int, Partner> Dict(FbConnection con)
        {
            Dictionary<int, Partner> dict = new Dictionary<int, Partner>();
            using (FbCommand cmd = new FbCommand(@"SELECT PPID, NAZIV, ADRESA, POSTA, MESTO, TELEFON, FAX,
                EMAIL, KONTAKT, PIB, KATEGORIJA, MBROJ, MOBILNI, MESTOID, AKTIVAN, PDVO,
                DRZAVAID, DUGUJE, POTRAZUJE FROM PARTNER", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
#pragma warning disable CS8601 // Possible null reference assignment.
                        dict.Add(Convert.ToInt32(dr["PPID"]), new Partner()
                        {
                            PPID = Convert.ToInt32(dr["PPID"]),
                            Naziv = Convert.ToString(dr["NAZIV"]),
                            Adresa = dr["ADRESA"] is DBNull ? null : Convert.ToString(dr["ADRESA"]),
                            Posta = dr["POSTA"] is DBNull ? null : Convert.ToString(dr["POSTA"]),
                            MestoID = dr["MESTOID"] is DBNull ? null : Convert.ToString(dr["MESTOID"]),
                            Telefon = dr["TELEFON"] is DBNull ? null : Convert.ToString(dr["TELEFON"]),
                            Fax = dr["FAX"] is DBNull ? null : Convert.ToString(dr["FAX"]),
                            Email = dr["EMAIL"] is DBNull ? null : Convert.ToString(dr["EMAIL"]),
                            Kontakt = dr["KONTAKT"] is DBNull ? null : Convert.ToString(dr["KONTAKT"]),
                            PIB = dr["PIB"] is DBNull ? null : Convert.ToString(dr["PIB"]),
                            Kategorija = dr["KATEGORIJA"] is DBNull ? (Int64?)null : Convert.ToInt64(dr["KATEGORIJA"]),
                            MBroj = dr["MBROJ"] is DBNull ? null : Convert.ToString(dr["MBROJ"]),
                            Mobilni = dr["MOBILNI"] is DBNull ? null : Convert.ToString(dr["MOBILNI"]),
                            Aktivan = Convert.ToInt32(dr["AKTIVAN"]),
                            PDVO = Convert.ToInt32(dr["PDVO"]),
                            DrzavaID = dr["DRZAVAID"] is DBNull ? 0 : Convert.ToInt32(dr["DRZAVAID"]),
                            Duguje = Convert.ToDouble(dr["DUGUJE"]),
                            Potrazuje = Convert.ToDouble(dr["POTRAZUJE"]),
                        });
#pragma warning restore CS8601 // Possible null reference assignment.
            }
            return dict;
        }
    }
}
