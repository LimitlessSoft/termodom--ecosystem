using FirebirdSql.Data.FirebirdClient;
using System.Collections;
using System.Collections.ObjectModel;

namespace TDBrain_v3.DB.Komercijalno
{

    public class Stavka
    {
        /// <summary>
        ///
        /// </summary>
        public class StavkaCollection : IEnumerable<Stavka>
        {
            private Dictionary<int, Stavka> _dict { get; set; }

            /// <summary>
            /// Vraca Stavku iz kolekcije sa zadatim stavkaID-om.
            /// </summary>
            /// <param name="stavkaID"></param>
            /// <exception cref="KeyNotFoundException"></exception>
            /// <returns></returns>
            public Stavka this[int stavkaID]
            {
                get
                {
                    return _dict[stavkaID];
                }
            }

            /// <summary>
            /// Kreira kolekciju stavki
            /// </summary>
            /// <param name="dict"></param>
            public StavkaCollection(Dictionary<int, Stavka> dict)
            {
                _dict = dict;
            }

            /// <summary>
            /// Vraca enumerator nad vrednostima stavka kolekcije
            /// </summary>
            /// <returns></returns>
            public IEnumerator<Stavka> GetEnumerator()
            {
                return _dict.Values.GetEnumerator();
            }

            /// <summary>
            /// Vraca enumerator nad vrednostima stavka kolekcije
            /// </summary>
            /// <returns></returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return _dict.Values.GetEnumerator();
            }
        }

        #region Properties
        public int StavkaID { get; set; }
        public int VrDok { get; set; }
        public int BrDok { get; set; }
        public int MagacinID { get; set; }
        public int RobaID { get; set; }
        public int? Vrsta { get; set; }
        public string? Naziv { get; set; }
        public double? NabCenSaPor { get; set; }
        public double? FakturnaCena { get; set; }
        public double? NabCenaBT { get; set; }
        public double? Troskovi { get; set; }
        public double NabavnaCena { get; set; }
        public double ProdCenaBP { get; set; }
        public double? Korekcija { get; set; }
        public double ProdajnaCena { get; set; }
        public double DeviznaCena { get; set; }
        public double? DevProdCena { get; set; }
        public double Kolicina { get; set; }
        public double NivKol { get; set; }
        public string? TarifaID { get; set; }
        public int? ImaPorez { get; set; }
        public double Porez { get; set; }
        public double Rabat { get; set; }
        public double Marza { get; set; }
        public double? Taksa { get; set; }
        public double? Akciza { get; set; }
        public double ProsNab { get; set; }
        public double PreCena { get; set; }
        public double PreNab { get; set; }
        public double ProsProd { get; set; }
        public string? MTID { get; set; }
        public char PT { get; set; }
        public string? Zvezdica { get; set; }
        public double? TrenStanje { get; set; }
        public double PorezUlaz { get; set; }
        public DateTime? SDatum { get; set; }
        public double? DevNabCena { get; set; }
        public double PorezIz { get; set; }
        public double? X4 { get; set; }
        public double? Y4 { get; set; }
        public double? Z4 { get; set; }
        public double? CenaPoAJM { get; set; }
        public int? KGID { get; set; }
        public double SAkciza { get; set; }
        #endregion

        /// <summary>
        /// Azurira vrednosti stavke u bazi
        /// </summary>
        /// <param name="con"></param>
        public void Update(FbConnection con)
        {
            using (FbCommand cmd = new FbCommand(@"UPDATE STAVKA SET
                                                        VRSTA = @VRSTA,
                                                        NAZIV = @NAZIV,
                                                        NABCENSAPOR = @NABCENSAPOR,
                                                        FAKTURNACENA = @FAKTURNACENA,
                                                        NABCENABT = @NABCENABT,
                                                        TROSKOVI = @TROSKOVI,
                                                        NABAVNACENA = @NABAVNACENA,
                                                        PRODCENABP = @PRODCENABP,
                                                        KOREKCIJA = @KOREKCIJA,
                                                        PRODAJNACENA = @PRODAJNACENA,
                                                        DEVIZNACENA = @DEVIZNACENA,
                                                        KOLICINA = @KOLICINA,
                                                        NIVKOL = @NIVKOL,
                                                        TARIFAID = @TARIFAID,
                                                        IMAPOREZ = @IMAPOREZ,
                                                        POREZ = @POREZ,
                                                        RABAT = @RABAT,
                                                        MARZA = @MARZA,
                                                        TAKSA = @TAKSA,
                                                        AKCIZA = @AKCIZA,
                                                        PROSNAB = @PROSNAB,
                                                        PRECENA = @PRECENA,
                                                        PRENAB = @PRENAB,
                                                        PROSPROD = @PROSPROD,
                                                        MTID = @MTID,
                                                        PT = @PT,
                                                        ZVEZDICA = @ZVEZDICA,
                                                        TREN_STANJE = @TREN_STANJE,
                                                        POREZ_ULAZ = @POREZ_ULAZ,
                                                        SDATUM = @SDATUM,
                                                        DEVNABCENA = @DEVNABCENA,
                                                        POREZ_IZ = @POREZ_IZ,
                                                        X4 = @X4,
                                                        Y4 = @Y4,
                                                        Z4 = @Z4,
                                                        CENAPOAJM = @CENAPOAJM,
                                                        KGID = @KGID,
                                                        SAKCIZA = @SAKCIZA
                                                        WHERE STAVKAID = @SID", con))
            {
                cmd.Parameters.AddWithValue("@VRSTA", Vrsta);
                cmd.Parameters.AddWithValue("@NAZIV", Naziv);
                cmd.Parameters.AddWithValue("@NABCENSAPOR", NabCenSaPor);
                cmd.Parameters.AddWithValue("@FAKTURNACENA", FakturnaCena);
                cmd.Parameters.AddWithValue("@NABCENABT", NabCenaBT);
                cmd.Parameters.AddWithValue("@TROSKOVI", Troskovi);
                cmd.Parameters.AddWithValue("@NABAVNACENA", NabavnaCena);
                cmd.Parameters.AddWithValue("@PRODCENABP", ProdCenaBP);
                cmd.Parameters.AddWithValue("@KOREKCIJA", Korekcija);
                cmd.Parameters.AddWithValue("@PRODAJNACENA", ProdajnaCena);
                cmd.Parameters.AddWithValue("@DEVIZNACENA", DeviznaCena);
                cmd.Parameters.AddWithValue("@KOLICINA", Kolicina);
                cmd.Parameters.AddWithValue("@NIVKOL", NivKol);
                cmd.Parameters.AddWithValue("@TARIFAID", TarifaID);
                cmd.Parameters.AddWithValue("@IMAPOREZ", ImaPorez);
                cmd.Parameters.AddWithValue("@POREZ", Porez);
                cmd.Parameters.AddWithValue("@RABAT", Rabat);
                cmd.Parameters.AddWithValue("@MARZA", Marza);
                cmd.Parameters.AddWithValue("@TAKSA", Taksa);
                cmd.Parameters.AddWithValue("@AKCIZA", Akciza);
                cmd.Parameters.AddWithValue("@PROSNAB", ProsNab);
                cmd.Parameters.AddWithValue("@PRECENA", PreCena);
                cmd.Parameters.AddWithValue("@PRENAB", PreNab);
                cmd.Parameters.AddWithValue("@PROSPROD", ProsProd);
                cmd.Parameters.AddWithValue("@MTID", MTID);
                cmd.Parameters.AddWithValue("@PT", PT);
                cmd.Parameters.AddWithValue("@ZVEZDICA", Zvezdica);
                cmd.Parameters.AddWithValue("@TREN_STANJE", TrenStanje);
                cmd.Parameters.AddWithValue("@POREZ_ULAZ", PorezUlaz);
                cmd.Parameters.AddWithValue("@SDATUM", SDatum);
                cmd.Parameters.AddWithValue("@DEVNABCENA", DevNabCena);
                cmd.Parameters.AddWithValue("@POREZ_IZ", PorezIz);
                cmd.Parameters.AddWithValue("@X4", X4);
                cmd.Parameters.AddWithValue("@Y4", Y4);
                cmd.Parameters.AddWithValue("@Z4", Z4);
                cmd.Parameters.AddWithValue("@CENAPOAJM", CenaPoAJM);
                cmd.Parameters.AddWithValue("@KGID", KGID);
                cmd.Parameters.AddWithValue("@SAKCIZA", SAkciza);
                cmd.Parameters.AddWithValue("@SID", StavkaID);

                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Vraca objekat stavke iz baze
        /// </summary>
        /// <param name="con"></param>
        /// <param name="stavkaID"></param>
        /// <returns></returns>
        public static Stavka? Get(FbConnection con, int stavkaID)
        {
            using (FbCommand cmd = new FbCommand(@"SELECT
                                                        STAVKAID,
                                                        VRDOK,
                                                        BRDOK,
                                                        MAGACINID,
                                                        ROBAID,
                                                        VRSTA,
                                                        NAZIV,
                                                        NABCENSAPOR,
                                                        FAKTURNACENA,
                                                        NABCENABT,
                                                        TROSKOVI,
                                                        NABAVNACENA,
                                                        PRODCENABP,
                                                        KOREKCIJA,
                                                        PRODAJNACENA,
                                                        DEVIZNACENA,
                                                        DEVPRODCENA,
                                                        KOLICINA,
                                                        NIVKOL,
                                                        TARIFAID,
                                                        IMAPOREZ,
                                                        POREZ,
                                                        RABAT,
                                                        MARZA,
                                                        TAKSA,
                                                        AKCIZA,
                                                        PROSNAB,
                                                        PRECENA,
                                                        PRENAB,
                                                        PROSPROD,
                                                        MTID,
                                                        PT,
                                                        ZVEZDICA,
                                                        TREN_STANJE,
                                                        POREZ_ULAZ,
                                                        SDATUM,
                                                        DEVNABCENA,
                                                        POREZ_IZ,
                                                        X4,
                                                        Y4,
                                                        Z4,
                                                        CENAPOAJM,
                                                        KGID,
                                                        SAKCIZA
                                                        FROM STAVKA
                                                        WHERE STAVKAID = " + stavkaID, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                        return new Stavka()
                        {
                            StavkaID = Convert.ToInt32(dr["STAVKAID"]),
                            VrDok = Convert.ToInt32(dr["VRDOK"]),
                            BrDok = Convert.ToInt32(dr["BRDOK"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            RobaID = Convert.ToInt32(dr["ROBAID"]),
                            Vrsta = dr["VRSTA"] is DBNull ? null : (int?)Convert.ToInt32(dr["VRSTA"]),
                            Naziv = dr["NAZIV"] is DBNull ? null : dr["NAZIV"].ToString(),
                            NabCenSaPor = dr["NABCENSAPOR"] is DBNull ? null : (double?)Convert.ToDouble(dr["NABCENSAPOR"]),
                            FakturnaCena = dr["FAKTURNACENA"] is DBNull ? null : (double?)Convert.ToDouble(dr["FAKTURNACENA"]),
                            NabCenaBT = dr["NABCENABT"] is DBNull ? null : (double?)Convert.ToDouble(dr["NABCENABT"]),
                            Troskovi = dr["TROSKOVI"] is DBNull ? null : (double?)Convert.ToDouble(dr["TROSKOVI"]),
                            NabavnaCena = Convert.ToDouble(dr["NABAVNACENA"]),
                            ProdCenaBP = Convert.ToDouble(dr["PRODCENABP"]),
                            Korekcija = dr["KOREKCIJA"] is DBNull ? null : (double?)Convert.ToDouble(dr["KOREKCIJA"]),
                            ProdajnaCena = Convert.ToDouble(dr["PRODAJNACENA"]),
                            DeviznaCena = Convert.ToDouble(dr["DEVIZNACENA"]),
                            DevProdCena = dr["DEVPRODCENA"] is DBNull ? null : (double?)Convert.ToDouble(dr["DEVPRODCENA"]),
                            Kolicina = Convert.ToDouble(dr["KOLICINA"]),
                            NivKol = Convert.ToDouble(dr["NIVKOL"]),
                            TarifaID = dr["TARIFAID"] is DBNull ? null : dr["TARIFAID"].ToString(),
                            ImaPorez = dr["IMAPOREZ"] is DBNull ? null : (int?)Convert.ToInt32(dr["IMAPOREZ"]),
                            Porez = Convert.ToDouble(dr["POREZ"]),
                            Rabat = Convert.ToDouble(dr["RABAT"]),
                            Marza = Convert.ToDouble(dr["MARZA"]),
                            Taksa = dr["TAKSA"] is DBNull ? null : (double?)Convert.ToDouble(dr["TAKSA"]),
                            Akciza = dr["AKCIZA"] is DBNull ? null : (double?)Convert.ToDouble(dr["AKCIZA"]),
                            ProsNab = Convert.ToDouble(dr["PROSNAB"]),
                            PreCena = Convert.ToDouble(dr["PRECENA"]),
                            PreNab = Convert.ToDouble(dr["PRENAB"]),
                            ProsProd = Convert.ToDouble(dr["PROSPROD"]),
                            MTID = dr["MTID"] is DBNull ? null : dr["MTID"].ToString(),
                            PT = Convert.ToChar(dr["PT"]),
                            Zvezdica = dr["ZVEZDICA"] is DBNull ? null : dr["ZVEZDICA"].ToString(),
                            TrenStanje = dr["TREN_STANJE"] is DBNull ? null : (double?)Convert.ToDouble(dr["TREN_STANJE"]),
                            PorezUlaz = Convert.ToDouble(dr["POREZ_ULAZ"]),
                            SDatum = dr["SDATUM"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["SDATUM"]),
                            DevNabCena = dr["DEVNABCENA"] is DBNull ? null : (double?)Convert.ToDouble(dr["DEVNABCENA"]),
                            PorezIz = Convert.ToDouble(dr["POREZ_IZ"]),
                            X4 = dr["X4"] is DBNull ? null : (double?)Convert.ToDouble(dr["X4"]),
                            Y4 = dr["Y4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Y4"]),
                            Z4 = dr["Z4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Z4"]),
                            CenaPoAJM = dr["CENAPOAJM"] is DBNull ? null : (double?)Convert.ToDouble(dr["CENAPOAJM"]),
                            KGID = dr["KGID"] is DBNull ? null : (int?)Convert.ToInt32(dr["KGID"]),
                            SAkciza = Convert.ToDouble(dr["SAKCIZA"])
                        };
                }
            }

            return null;
        }
        /// <summary>
        /// Dodaje novu stavku u bazu
        /// </summary>
        /// <param name="dokument"></param>
        /// <param name="roba"></param>
        /// <param name="robaUMagacinu"></param>
        /// <param name="kolicina"></param>
        /// <param name="rabat"></param>
        /// <param name="prodajnaCenaBezPDV"></param>
        /// <returns></returns>
        public static int Insert(int godina, int magacinId, Dokument dokument, Roba roba, RobaUMagacinu robaUMagacinu, double? kolicina, double rabat, double? prodajnaCenaBezPDV = null)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[magacinId, godina]))
            {
                con.Open();
                return Insert(con, dokument, roba, robaUMagacinu, kolicina, rabat, prodajnaCenaBezPDV);
            }
        }
        /// <summary>
        /// Dodaje novu stavku u bazu
        /// </summary>
        /// <param name="con"></param>
        /// <param name="dokument"></param>
        /// <param name="roba"></param>
        /// <param name="robaUMagacinu"></param>
        /// <param name="kolicina"></param>
        /// <param name="rabat"></param>
        /// <param name="prodajnaCenaBezPDV"></param>
        /// <returns></returns>
        public static int Insert(FbConnection con, Dokument dokument, Roba roba, RobaUMagacinu robaUMagacinu, double? kolicina, double rabat, double? prodajnaCenaBezPDV = null)
        {
            List<Tarife> tarife = Tarife.List(con);
            Magacin? mag = Magacin.Get(DateTime.Now.Year, dokument.MagacinID);

            if(mag == null)
                throw new Exception("Greska prilikom selektovanja magacina!");

            using (FbCommand cmd = new FbCommand(@"INSERT INTO STAVKA
            (VRDOK, BRDOK, MAGACINID, ROBAID, VRSTA, NAZIV, NABCENSAPOR, FAKTURNACENA, NABCENABT,
            TROSKOVI, NABAVNACENA, PRODCENABP, KOREKCIJA, PRODAJNACENA, DEVIZNACENA, DEVPRODCENA, KOLICINA,
            NIVKOL, TARIFAID, IMAPOREZ, POREZ, RABAT, MARZA, TAKSA, AKCIZA, PROSNAB, PRECENA, PRENAB, PROSPROD,
            MTID, PT, TREN_STANJE, POREZ_ULAZ, DEVNABCENA, POREZ_IZ)
            VALUES (@VRDOK, @BRDOK, @MAGACINID, @ROBAID, 1, @NAZIV, 0, 0, 0, 
            0, @NABAVNACENA, @CENA_BEZ_PDV, 0, @CENA_SA_PDV, 0, 0, @KOL,
            0, @TARIFAID, 0, @POREZ, @RABAT, 0, 0, 0, 0, 0, 0, 0, 
            @MTID, 'P', 0, 0, 0, @POREZ) RETURNING STAVKAID", con))
            {
                double stopa = Convert.ToDouble(tarife.First(x => x.TarifaID == roba.TarifaID).Stopa);
                cmd.Parameters.AddWithValue("@VRDOK", dokument.VrDok);
                cmd.Parameters.AddWithValue("@BRDOK", dokument.BrDok);
                cmd.Parameters.AddWithValue("@MAGACINID", dokument.MagacinID);
                cmd.Parameters.AddWithValue("@ROBAID", roba.ID);
                cmd.Parameters.AddWithValue("@NAZIV", roba.Naziv);
                cmd.Parameters.AddWithValue("@NABAVNACENA", robaUMagacinu.NabavnaCena);
                cmd.Parameters.AddWithValue("@CENA_SA_PDV", prodajnaCenaBezPDV == null ?
                    Procedure.ProdajnaCenaNaDan(dokument.MagacinID, DateTime.Now.Year, roba.ID, dokument.Datum) :
                    prodajnaCenaBezPDV * (1 + (stopa / 100)));
                cmd.Parameters.AddWithValue("@CENA_BEZ_PDV", prodajnaCenaBezPDV == null ?
                    (double)cmd.Parameters["@CENA_SA_PDV"].Value * (1 - (stopa / (100 + stopa))) :
                    Procedure.ProdajnaCenaNaDan(dokument.MagacinID, DateTime.Now.Year, roba.ID, dokument.Datum));
                cmd.Parameters.AddWithValue("@KOL", kolicina);
                cmd.Parameters.AddWithValue("@TARIFAID", roba.TarifaID);
                cmd.Parameters.AddWithValue("@POREZ", stopa);
                cmd.Parameters.AddWithValue("@RABAT", rabat);
                cmd.Parameters.AddWithValue("@MTID", mag.MTID);

                cmd.Parameters.Add(new FbParameter { ParameterName = "STAVKAID", FbDbType = FbDbType.Integer, Direction = System.Data.ParameterDirection.Output });

                cmd.ExecuteNonQuery();

                return Convert.ToInt32(cmd.Parameters["STAVKAID"].Value);
            }
        }
        public static List<Stavka> List(FbConnection con, List<string>? queryParameters = null)
        {
            return Dict(con, queryParameters).ToList();
        }
        /// <summary>
        /// Vraca kolekciju stavki iz baze
        /// </summary>
        /// <param name="con"></param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        public static StavkaCollection Dict(FbConnection con, List<string>? queryParameters = null)
        {
            string whereQuery = "";

            if (queryParameters != null && queryParameters.Count > 0)
                whereQuery = " WHERE " + string.Join(" AND ", queryParameters);

            Dictionary<int, Stavka> dict = new Dictionary<int, Stavka>();

            using (FbCommand cmd = new FbCommand(@"SELECT
                                                        STAVKAID,
                                                        VRDOK,
                                                        BRDOK,
                                                        MAGACINID,
                                                        ROBAID,
                                                        VRSTA,
                                                        NAZIV,
                                                        NABCENSAPOR,
                                                        FAKTURNACENA,
                                                        NABCENABT,
                                                        TROSKOVI,
                                                        NABAVNACENA,
                                                        PRODCENABP,
                                                        KOREKCIJA,
                                                        PRODAJNACENA,
                                                        DEVIZNACENA,
                                                        DEVPRODCENA,
                                                        KOLICINA,
                                                        NIVKOL,
                                                        TARIFAID,
                                                        IMAPOREZ,
                                                        POREZ,
                                                        RABAT,
                                                        MARZA,
                                                        TAKSA,
                                                        AKCIZA,
                                                        PROSNAB,
                                                        PRECENA,
                                                        PRENAB,
                                                        PROSPROD,
                                                        MTID,
                                                        PT,
                                                        ZVEZDICA,
                                                        TREN_STANJE,
                                                        POREZ_ULAZ,
                                                        SDATUM,
                                                        DEVNABCENA,
                                                        POREZ_IZ,
                                                        X4,
                                                        Y4,
                                                        Z4,
                                                        CENAPOAJM,
                                                        KGID,
                                                        SAKCIZA
                                                        FROM STAVKA " + whereQuery, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        dict.Add(Convert.ToInt32(dr["STAVKAID"]), new Stavka()
                        {
                            StavkaID = Convert.ToInt32(dr["STAVKAID"]),
                            VrDok = Convert.ToInt32(dr["VRDOK"]),
                            BrDok = Convert.ToInt32(dr["BRDOK"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            RobaID = Convert.ToInt32(dr["ROBAID"]),
                            Vrsta = dr["VRSTA"] is DBNull ? null : (int?)Convert.ToInt32(dr["VRSTA"]),
                            Naziv = dr["NAZIV"] is DBNull ? null : dr["NAZIV"].ToString(),
                            NabCenSaPor = dr["NABCENSAPOR"] is DBNull ? null : (double?)Convert.ToDouble(dr["NABCENSAPOR"]),
                            FakturnaCena = dr["FAKTURNACENA"] is DBNull ? null : (double?)Convert.ToDouble(dr["FAKTURNACENA"]),
                            NabCenaBT = dr["NABCENABT"] is DBNull ? null : (double?)Convert.ToDouble(dr["NABCENABT"]),
                            Troskovi = dr["TROSKOVI"] is DBNull ? null : (double?)Convert.ToDouble(dr["TROSKOVI"]),
                            NabavnaCena = Convert.ToDouble(dr["NABAVNACENA"]),
                            ProdCenaBP = Convert.ToDouble(dr["PRODCENABP"]),
                            Korekcija = dr["KOREKCIJA"] is DBNull ? null : (double?)Convert.ToDouble(dr["KOREKCIJA"]),
                            ProdajnaCena = Convert.ToDouble(dr["PRODAJNACENA"]),
                            DeviznaCena = Convert.ToDouble(dr["DEVIZNACENA"]),
                            DevProdCena = dr["DEVPRODCENA"] is DBNull ? null : (double?)Convert.ToDouble(dr["DEVPRODCENA"]),
                            Kolicina = Convert.ToDouble(dr["KOLICINA"]),
                            NivKol = Convert.ToDouble(dr["NIVKOL"]),
                            TarifaID = dr["TARIFAID"] is DBNull ? null : dr["TARIFAID"].ToString(),
                            ImaPorez = dr["IMAPOREZ"] is DBNull ? null : (int?)Convert.ToInt32(dr["IMAPOREZ"]),
                            Porez = Convert.ToDouble(dr["POREZ"]),
                            Rabat = Convert.ToDouble(dr["RABAT"]),
                            Marza = Convert.ToDouble(dr["MARZA"]),
                            Taksa = dr["TAKSA"] is DBNull ? null : (double?)Convert.ToDouble(dr["TAKSA"]),
                            Akciza = dr["AKCIZA"] is DBNull ? null : (double?)Convert.ToDouble(dr["AKCIZA"]),
                            ProsNab = Convert.ToDouble(dr["PROSNAB"]),
                            PreCena = Convert.ToDouble(dr["PRECENA"]),
                            PreNab = Convert.ToDouble(dr["PRENAB"]),
                            ProsProd = Convert.ToDouble(dr["PROSPROD"]),
                            MTID = dr["MTID"] is DBNull ? null : dr["MTID"].ToString(),
                            PT = Convert.ToChar(dr["PT"]),
                            Zvezdica = dr["ZVEZDICA"] is DBNull ? null : dr["ZVEZDICA"].ToString(),
                            TrenStanje = dr["TREN_STANJE"] is DBNull ? null : (double?)Convert.ToDouble(dr["TREN_STANJE"]),
                            PorezUlaz = Convert.ToDouble(dr["POREZ_ULAZ"]),
                            SDatum = dr["SDATUM"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["SDATUM"]),
                            DevNabCena = dr["DEVNABCENA"] is DBNull ? null : (double?)Convert.ToDouble(dr["DEVNABCENA"]),
                            PorezIz = Convert.ToDouble(dr["POREZ_IZ"]),
                            X4 = dr["X4"] is DBNull ? null : (double?)Convert.ToDouble(dr["X4"]),
                            Y4 = dr["Y4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Y4"]),
                            Z4 = dr["Z4"] is DBNull ? null : (double?)Convert.ToDouble(dr["Z4"]),
                            CenaPoAJM = dr["CENAPOAJM"] is DBNull ? null : (double?)Convert.ToDouble(dr["CENAPOAJM"]),
                            KGID = dr["KGID"] is DBNull ? null : (int?)Convert.ToInt32(dr["KGID"]),
                            SAkciza = Convert.ToDouble(dr["SAKCIZA"])
                        });
                }
            }

            return new StavkaCollection(dict);
        }
        
    }
}
