using FirebirdSql.Data.FirebirdClient;

namespace TDBrain_v3.DB.Komercijalno
{
    /// <summary>
    /// Klasa za komunikaciju sa tabelom Dokument.
    /// </summary>
    public class Dokument
    {
        /// <summary>
        /// Vrsta dokumenta
        /// </summary>
        public int VrDok { get; set; }
        /// <summary>
        /// Broj dokumenta
        /// </summary>
        public int BrDok { get; set; }
        /// <summary>
        /// Interni Broj dokumenta
        /// </summary>
        public string? IntBroj { get; set; }
        /// <summary>
        /// Flag dokumenta. 0 = Otkljucan, 1 = Zakljucan
        /// </summary>
        public int Flag { get; set; }
        /// <summary>
        /// Buffer polje fiskalizacije. 0 = nije fiskalizovan, 1 = fiskalizovan. Ne mora biti u pravu.
        /// </summary>
        public int Placen { get; set; }
        /// <summary>
        /// Datum dokumenta
        /// </summary>
        public DateTime Datum { get; set; }
        /// <summary>
        /// Datum roka dokumenta
        /// </summary>
        public DateTime? DatRoka { get; set; }
        /// <summary>
        /// Magacin ID dokumenta
        /// </summary>
        public int MagacinID { get; set; }
        /// <summary>
        /// Mesto troska ID dokumenta
        /// </summary>
        public string? MTID { get; set; }
        /// <summary>
        /// Referent ID dokumenta
        /// </summary>
        public int RefID { get; set; }
        /// <summary>
        /// Zaposleni ID dokumenta
        /// </summary>
        public int ZapID { get; set; }
        /// <summary>
        /// KodDok dokumenta
        /// </summary>
        public int KodDok { get; set; }
        /// <summary>
        /// Duguje dokumenta
        /// </summary>
        public double Duguje { get; set; }
        /// <summary>
        /// Potrazuje dokumenta
        /// </summary>
        public double Potrazuje { get; set; }
        /// <summary>
        /// PPID dokumenta
        /// </summary>
        public int? PPID { get; set; }
        /// <summary>
        /// Nacin uplate ID. 1 = Virmanom, 5 = Gotovina
        /// </summary>
        public int NUID { get; set; }
        /// <summary>
        /// Troskovi dokumenta
        /// </summary>
        public double Troskovi { get; set; }
        /// <summary>
        /// Vrsta dokumenta iz koje je ovaj dokument prebacen
        /// </summary>
        public int? VrDokIn { get; set; }
        /// <summary>
        /// Broj dokumenta iz koje je ovaj dokument prebacen
        /// </summary>
        public int? BrDokIn { get; set; }
        /// <summary>
        /// Vrsta dokumenta u koji je ovaj dokument prebacen
        /// </summary>
        public int? VrDokOut { get; set; }
        /// <summary>
        /// Broj dokumenta u koji je ovaj dokument prebacen
        /// </summary>
        public int? BrDokOut { get; set; }
        /// <summary>
        /// Ne znam sta je ovo
        /// </summary>
        public int? MagID { get; set; }
        /// <summary>
        /// Alias U dokumenta
        /// </summary>
        public int? AliasU { get; set; }
        /// <summary>
        /// Opis uplate dokumenta
        /// </summary>
        public string? OpisUpl { get; set; }
        /// <summary>
        /// Popust 1 Dana dokumenta
        /// </summary>
        public int? Popust1Dana { get; set; }
        /// <summary>
        /// Razlika dokumenta
        /// </summary>
        public double Razlika { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[ {VrDok}, {BrDok} ]";
        }

        /// <summary>
        /// Azurira informacije objekta u bazu.
        /// </summary>
        public void Update(int godina, int magacinID)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[magacinID, godina]))
            {
                con.Open();
                Update(con);
            }
        }
        /// <summary>
        /// Azurira informacije objekta u bazu.
        /// </summary>
        /// <param name="con"></param>
        public void Update(FbConnection con)
        {
            using(FbCommand cmd = new FbCommand(@"
UPDATE DOKUMENT SET
OPISUPL = @OPISUPL,
ALIASU = @ALIASU
WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK", con))
            {
                cmd.Parameters.AddWithValue("@VRDOK", VrDok);
                cmd.Parameters.AddWithValue("@BRDOK", BrDok);
                cmd.Parameters.AddWithValue("@OPISUPL", OpisUpl);
                cmd.Parameters.AddWithValue("@ALIASU", AliasU);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Pravi novi dokument unutar baze tabela DOKUMENT i vraca novi broj dokumenta
        /// </summary>
        /// <param name="godina">Godina u kojoj se unosi dokument</param>
        /// <param name="vrDok">Vrsta za dokument koji se kreira</param>
        /// <param name="interniBroj">Interni broj dokumenta</param>
        /// <param name="ppid">ID poslovnog partnera</param>
        /// <param name="napomena">Napomena dokumenta</param>
        /// <param name="nacinUplateID">Nacin uplate dokumenta</param>
        /// <param name="magacinID">Magacin ID dokumenta</param>
        /// <param name="komercijalnoKorisnikID">ID korisnika u komercijalnom poslovanju</param>
        /// <param name="magID">Mag ID</param>
        /// <returns></returns>
        /// 
        public static int Insert(int godina,
            int vrDok,
            string? interniBroj,
            int? ppid,
            string napomena,
            int nacinUplateID,
            int magacinID,
            int? komercijalnoKorisnikID,
            int? magID)
        {
            using (FbConnection con =
                new FbConnection(DB.Settings.ConnectionStringKomercijalno[magacinID, godina]))
            {
                con.Open();
                return Insert(con, vrDok,
                    interniBroj, ppid, napomena, nacinUplateID,
                    magacinID, komercijalnoKorisnikID, magID);
            }
        }
        /// <summary>
        /// Pravi novi dokument unutar baze tabela DOKUMENT i vraca novi broj dokumenta
        /// </summary>
        /// <param name="con"></param>
        /// <param name="vrDok"></param>
        /// <param name="interniBroj"></param>
        /// <param name="ppid"></param>
        /// <param name="napomena"></param>
        /// <param name="nacinUplateID"></param>
        /// <param name="magacinID"></param>
        /// <param name="komercijalnoKorisnikID"></param>
        /// <param name="magID"></param>
        /// <param name="dozvoliDaljeIzmeneUKomercijalnom"></param>
        /// <returns></returns>
        public static int Insert(FbConnection con,
            int vrDok,
            string? interniBroj,
            int? ppid,
            string napomena,
            int nacinUplateID,
            int magacinID,
            int? komercijalnoKorisnikID,
            int? magID,
            bool dozvoliDaljeIzmeneUKomercijalnom = false)
        {
            int poslednjiBrDok = 0;
            using (FbCommand cmd = new FbCommand(@"SELECT POSLEDNJI FROM VRSTADOKMAG WHERE VRDOK = @VRDOK AND MAGACINID = @MAGACINID", con))
            {
                cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                cmd.Parameters.AddWithValue("@MAGACINID", magacinID);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        poslednjiBrDok = Convert.ToInt32(dr["POSLEDNJI"]);
            }

            if (poslednjiBrDok == 0)
            {
                using (FbCommand cmd = new FbCommand(@"SELECT POSLEDNJI FROM VRSTADOK WHERE VRDOK = @VRDOK", con))
                {
                    cmd.Parameters.AddWithValue("@VRDOK", vrDok);

                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            poslednjiBrDok = Convert.ToInt32(dr["POSLEDNJI"]);
                }
            }

            using (FbCommand cmd = new FbCommand(@"INSERT INTO DOKUMENT (VRDOK, BRDOK, INTBROJ, KODDOK, FLAG, DATUM, LINKED, MAGACINID, PPID, PLACEN, DATROKA,
                                                    NUID, NRID, VALUTA, KURS, ZAPID, UPLACENO, TROSKOVI, DUGUJE, POTRAZUJE, POPUST, RAZLIKA,
                                                    DODPOREZ, POREZ, PRODVREDBP, VRDOKIN, BRDOKIN, MAGID, MTID, REFID, STATUS, PPO, PRENETI_POREZ,
                                                    AKVRDOK, AKBRDOK, PREVOZROBE, DATUM_PDV, NDID, NABVREDNOST, KNJIZNAOZ, POVRATNICE,
                                                    SINHRO, STORNO, SMENAID)
                                                    VALUES (@VRDOK, @BRDOK, @INTERNIBROJ, 0, 1, @DATUM, @LINKED, @MAGACINID, @PPID, 0, @DATUM,
                                                    @NUID, 1, (SELECT VALUTA FROM VRSTADOK WHERE VRDOK = @VRDOK), 1, @ZAPID, 0, 0, 0, 0, 0, 0,
                                                    0, 0, 0, @VRDOKIN, NULL, @MAGID, @MTID, @ZAPID, 0, 1, 0,
                                                    0, 0, @PREVOZROBE, @DATUM, 0, 0, 0, 0,
                                                    0, 0, 0)", con))
            {
                Magacin? mag = Magacin.Get(DateTime.Now.Year, magacinID);
                    
                if (mag == null)
                    throw new Exception("Magacin ne postoji!");

                cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                cmd.Parameters.AddWithValue("@MAGID", magID);
                cmd.Parameters.AddWithValue("@BRDOK", poslednjiBrDok + 1);
                cmd.Parameters.AddWithValue("@INTERNIBROJ", interniBroj);
                cmd.Parameters.AddWithValue("@DATUM", DateTime.Now);
                cmd.Parameters.AddWithValue("@LINKED", Procedure.NextLinked(DateTime.Now, magacinID));
                cmd.Parameters.AddWithValue("@MAGACINID", magacinID);
                cmd.Parameters.AddWithValue("@MTID", mag.MTID);
                cmd.Parameters.AddWithValue("@PPID", ppid);
                cmd.Parameters.AddWithValue("@NUID", nacinUplateID);
                cmd.Parameters.AddWithValue("@ZAPID", komercijalnoKorisnikID == null ? 1 : komercijalnoKorisnikID);
                cmd.Parameters.AddWithValue("@PREVOZROBE", ppid == null ? 0 : 1);
                cmd.Parameters.AddWithValue("@VRDOKIN", dozvoliDaljeIzmeneUKomercijalnom ? (int?)null : 8);

                cmd.ExecuteNonQuery();

                return poslednjiBrDok + 1;
            }
        }

        /// <summary>
        /// Vraca dokument iz baze
        /// </summary>
        /// <param name="magacinID">Magacin za koji se trazi dokument. Ukoliko se prosledi null onda trazi u svim bazama za datu godinu.</param>
        /// <param name="godina">Godina u kojoj se trazi dokument.</param>
        /// <param name="vrDok"></param>
        /// <param name="brDok"></param>
        /// <returns></returns>
        public static Dokument? Get(int? magacinID, int godina, int vrDok, int brDok)
        {
            if (magacinID == null)
                return List(godina, null).FirstOrDefault(x => x.VrDok == vrDok && x.BrDok == brDok);

            using (FbConnection con =
                new FbConnection(DB.Settings.ConnectionStringKomercijalno[(int)magacinID, godina]))
            {
                con.Open();
                return Get(con, vrDok, brDok);
            }
        }
        /// <summary>
        /// Vraca dokument iz baze
        /// </summary>
        /// <param name="con"></param>
        /// <param name="vrDok"></param>
        /// <param name="brDok"></param>
        /// <returns></returns>
        public static Dokument? Get(FbConnection con, int vrDok, int brDok)
        {
            using (FbCommand cmd = new FbCommand(@"SELECT * FROM DOKUMENT WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK", con))
            {
                cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                cmd.Parameters.AddWithValue("@BRDOK", brDok);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new Dokument()
                        {
                            VrDok = Convert.ToInt32(dr["VRDOK"]),
                            BrDok = Convert.ToInt32(dr["BRDOK"]),
                            Datum = Convert.ToDateTime(dr["DATUM"]),
                            DatRoka = dr["DATROKA"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["DATROKA"]),
                            Flag = Convert.ToInt32(dr["FLAG"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            IntBroj = dr["INTBROJ"].ToString(),
                            KodDok = Convert.ToInt32(dr["KODDOK"]),
                            Duguje = Convert.ToDouble(dr["DUGUJE"]),
                            MTID = dr["MTID"].ToString(),
                            Potrazuje = Convert.ToDouble(dr["POTRAZUJE"]),
                            Razlika = Convert.ToDouble(dr["RAZLIKA"]),
                            NUID = Convert.ToInt32(dr["NUID"]),
                            PPID = dr["PPID"] is DBNull ? null : (int?)Convert.ToInt32(dr["PPID"]),
                            Troskovi = dr["TROSKOVI"] is DBNull ? 0 : Convert.ToDouble(dr["TROSKOVI"]),
                            VrDokOut = dr["VRDOKOUT"] is DBNull ? 0 : Convert.ToInt32(dr["VRDOKOUT"]),
                            VrDokIn = dr["VRDOKIN"] is DBNull ? 0 : Convert.ToInt32(dr["VRDOKIN"]),
                            BrDokOut = dr["BRDOKOUT"] is DBNull ? 0 : Convert.ToInt32(dr["BRDOKOUT"]),
                            BrDokIn = dr["BRDOKIN"] is DBNull ? 0 : Convert.ToInt32(dr["BRDOKIN"]),
                            RefID = Convert.ToInt32(dr["REFID"]),
                            ZapID = Convert.ToInt32(dr["ZAPID"]),
                            Placen = Convert.ToInt32(dr["PLACEN"]),
                            MagID = dr["MAGID"] is DBNull ? null : (Int16?)Convert.ToInt16(dr["MAGID"]),
                            AliasU = dr["ALIASU"] is DBNull ? null : (int?)Convert.ToInt32(dr["ALIASU"]),
                            OpisUpl = dr["OPISUPL"] is DBNull ? null : dr["OPISUPL"].ToString(),
                            Popust1Dana = dr["POPUST1DANA"] is DBNull ? null : (int?)Convert.ToInt32(dr["POPUST1DANA"]),
                        };
            }
            return null;
        }
        /// <summary>
        /// Vraca listu dokumenata iz baza.
        /// </summary>
        /// <param name="godina">Godina za koju se vraca lista dokumenata. Ukoliko se prosledi null, hvata se trenutna godina.</param>
        /// <param name="magaciniID">Magacini za koje se vraca lista dokumenata. Ako se prosledi null, vraca se lista za sve magacine</param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        public static List<Dokument> List(int? godina = null, int[]? magaciniID = null, List<string>? queryParameters = null)
        {
            List<Dokument> list = new List<Dokument>();

            if (godina == null)
                godina = DateTime.Now.Year;

            if (magaciniID == null || magaciniID.Length == 0)
                magaciniID = DB.Settings.ConnectionStringKomercijalno.GetMagacini((int)godina);

            foreach(int magacin in magaciniID)
            {
                using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[magacin, (int)godina]))
                {
                    con.Open();
                    list.AddRange(List(con, queryParameters));
                }
            }

            return list;
        }
        /// <summary>
        /// Vraca listu dokumenata iz baze
        /// </summary>
        /// <param name="con"></param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        public static List<Dokument> List(FbConnection con, List<string>? queryParameters = null)
        {
            string whereQuery = "";

            if (queryParameters != null && queryParameters.Count > 0)
                whereQuery = " WHERE " + string.Join(" AND ", queryParameters);

            List<Dokument> dok = new List<Dokument>();
            using (FbCommand cmd = new FbCommand(@"SELECT * FROM DOKUMENT " + whereQuery, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        dok.Add(new Dokument()
                        {
                            VrDok = Convert.ToInt32(dr["VRDOK"]),
                            BrDok = Convert.ToInt32(dr["BRDOK"]),
                            Datum = Convert.ToDateTime(dr["DATUM"]),
                            DatRoka = dr["DATROKA"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["DATROKA"]),
                            Flag = Convert.ToInt32(dr["FLAG"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            IntBroj = dr["INTBROJ"].ToString(),
                            KodDok = Convert.ToInt32(dr["KODDOK"]),
                            Duguje = Convert.ToDouble(dr["DUGUJE"]),
                            MTID = dr["MTID"].ToString(),
                            Potrazuje = Convert.ToDouble(dr["POTRAZUJE"]),
                            Razlika = Convert.ToDouble(dr["RAZLIKA"]),
                            NUID = Convert.ToInt32(dr["NUID"]),
                            PPID = dr["PPID"] is DBNull ? null : (int?)Convert.ToInt32(dr["PPID"]),
                            Troskovi = dr["TROSKOVI"] is DBNull ? 0 : Convert.ToDouble(dr["TROSKOVI"]),
                            VrDokOut = dr["VRDOKOUT"] is DBNull ? 0 : Convert.ToInt32(dr["VRDOKOUT"]),
                            VrDokIn = dr["VRDOKIN"] is DBNull ? 0 : Convert.ToInt32(dr["VRDOKIN"]),
                            BrDokOut = dr["BRDOKOUT"] is DBNull ? 0 : Convert.ToInt32(dr["BRDOKOUT"]),
                            BrDokIn = dr["BRDOKIN"] is DBNull ? 0 : Convert.ToInt32(dr["BRDOKIN"]),
                            RefID = Convert.ToInt32(dr["REFID"]),
                            ZapID = Convert.ToInt32(dr["ZAPID"]),
                            Placen = Convert.ToInt32(dr["PLACEN"]),
                            MagID = dr["MAGID"] is DBNull ? null : (Int16?)Convert.ToInt16(dr["MAGID"]),
                            AliasU = dr["ALIASU"] is DBNull ? null : (int?)Convert.ToInt32(dr["ALIASU"]),
                            OpisUpl = dr["OPISUPL"] is DBNull ? null : dr["OPISUPL"].ToString(),
                            Popust1Dana = dr["POPUST1DANA"] is DBNull ? null : (int?)Convert.ToInt32(dr["POPUST1DANA"]),
                        });
            }
            return dok;
        }
        /// <summary>
        /// Vraca DokumentDictionary selektovanih objekata iz baze prosledjene konekcije
        /// </summary>
        /// <param name="con"></param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        public static Termodom.Data.Entities.Komercijalno.DokumentDictionary Dictionary(FbConnection con, List<string>? queryParameters = null)
        {
            string whereQuery = "";

            if (queryParameters != null && queryParameters.Count > 0)
                whereQuery = " WHERE " + string.Join(" AND ", queryParameters);

            Dictionary<int, Dictionary<int, Termodom.Data.Entities.Komercijalno.Dokument>> dict = new Dictionary<int, Dictionary<int, Termodom.Data.Entities.Komercijalno.Dokument>>();
            using (FbCommand cmd = new FbCommand(@"SELECT * FROM DOKUMENT " + whereQuery, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                    {
                        if (!dict.ContainsKey(Convert.ToInt32(dr["VRDOK"])))
                            dict.Add(Convert.ToInt32(dr["VRDOK"]), new Dictionary<int, Termodom.Data.Entities.Komercijalno.Dokument>());

                        dict[Convert.ToInt32(dr["VRDOK"])].Add(Convert.ToInt32(dr["BRDOK"]), new Termodom.Data.Entities.Komercijalno.Dokument()
                        {
                            VrDok = Convert.ToInt32(dr["VRDOK"]),
                            BrDok = Convert.ToInt32(dr["BRDOK"]),
                            Datum = Convert.ToDateTime(dr["DATUM"]),
                            DatRoka = dr["DATROKA"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["DATROKA"]),
                            Flag = Convert.ToInt32(dr["FLAG"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            IntBroj = dr["INTBROJ"].ToString(),
                            KodDok = Convert.ToInt32(dr["KODDOK"]),
                            Duguje = Convert.ToDouble(dr["DUGUJE"]),
                            MTID = dr["MTID"].ToString(),
                            Potrazuje = Convert.ToDouble(dr["POTRAZUJE"]),
                            Razlika = Convert.ToDouble(dr["RAZLIKA"]),
                            NUID = Convert.ToInt32(dr["NUID"]),
                            PPID = dr["PPID"] is DBNull ? null : (int?)Convert.ToInt32(dr["PPID"]),
                            Troskovi = dr["TROSKOVI"] is DBNull ? 0 : Convert.ToDouble(dr["TROSKOVI"]),
                            VrDokOut = dr["VRDOKOUT"] is DBNull ? 0 : Convert.ToInt32(dr["VRDOKOUT"]),
                            VrDokIn = dr["VRDOKIN"] is DBNull ? 0 : Convert.ToInt32(dr["VRDOKIN"]),
                            BrDokOut = dr["BRDOKOUT"] is DBNull ? 0 : Convert.ToInt32(dr["BRDOKOUT"]),
                            BrDokIn = dr["BRDOKIN"] is DBNull ? 0 : Convert.ToInt32(dr["BRDOKIN"]),
                            RefID = Convert.ToInt32(dr["REFID"]),
                            ZapID = Convert.ToInt32(dr["ZAPID"]),
                            Placen = Convert.ToInt32(dr["PLACEN"]),
                            MagID = dr["MAGID"] is DBNull ? null : (Int16?)Convert.ToInt16(dr["MAGID"]),
                            AliasU = dr["ALIASU"] is DBNull ? null : (int?)Convert.ToInt32(dr["ALIASU"]),
                            OpisUpl = dr["OPISUPL"] is DBNull ? null : dr["OPISUPL"].ToString(),
                            Popust1Dana = dr["POPUST1DANA"] is DBNull ? null : (int?)Convert.ToInt32(dr["POPUST1DANA"]),
                        });
                    }
            }
            return new Termodom.Data.Entities.Komercijalno.DokumentDictionary(dict);
        }
    }
}
