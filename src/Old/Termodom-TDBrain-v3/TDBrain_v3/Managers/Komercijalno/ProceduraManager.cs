using FirebirdSql.Data.FirebirdClient;
using TDBrain_v3.DB.Komercijalno;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.Managers.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProceduraManager
    {
        /// <summary>
        /// Izvrsava stored proceduru za presabiranje dokumenta
        /// </summary>
        /// <param name="con"></param>
        /// <param name="vrDok"></param>
        /// <param name="brDok"></param>
        public static void PresaberiDokument(FbConnection con, int vrDok, int brDok)
        {
            using (FbCommand cmd = new FbCommand("PRESABERIDOKUMENT", con))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("VRDOK", vrDok);
                cmd.Parameters.AddWithValue("BRDOK", brDok);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="con"></param>
        /// <param name="magacinID"></param>
        /// <param name="robaID"></param>
        /// <param name="datum"></param>
        /// <returns></returns>
        public static double ProdajnaCenaNaDan(FbConnection con, int magacinID, int robaID, DateTime datum)
        {
            using (FbCommand cmd = new FbCommand("CENE_NA_DAN", con))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("MAGACINID", magacinID);
                cmd.Parameters.AddWithValue("ROBAID", robaID);
                cmd.Parameters.AddWithValue("DATUM", datum);

                cmd.Parameters.Add("PRODAJNACENA", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("NABAVNACENA", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;

                cmd.ExecuteScalar();

                return Math.Round(Convert.ToDouble(cmd.Parameters["PRODAJNACENA"].Value), 2);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datum"></param>
        /// <param name="magacinID"></param>
        /// <returns></returns>
        public static string NextLinked(FbConnection con, DateTime datum, int magacinID)
        {
            using (FbCommand cmd = new FbCommand(@"SELECT MAX(CAST(LINKED AS INTEGER)) FROM DOKUMENT
                                                    WHERE DATUM = @DATUM
                                                    AND MAGACINID = @MAGACINID
                                                    AND(LINKED <> '9999999999') AND(LINKED IS NOT NULL) AND(LINKED <> '')", con))
            {
                cmd.Parameters.AddWithValue("@DATUM", datum);
                cmd.Parameters.AddWithValue("@MAGACINID", magacinID);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return (dr[0] is DBNull ? 1 : Convert.ToInt32(dr[0])).ToString("0000000000");
            }

            return "0000000000";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="con"></param>
        /// <param name="vrDok"></param>
        /// <param name="brDok"></param>
        /// <param name="robaId"></param>
        /// <param name="cenaBezPdv"></param>
        /// <param name="kolicina"></param>
        /// <param name="rabat"></param>
        /// <returns></returns>
        public static int NapraviUslugu(FbConnection con, int vrDok, int brDok, int robaId, double cenaBezPdv, double kolicina, double rabat)
        {
            var dokument = DokumentManager.Get(con, vrDok, brDok);
            var roba = RobaManager.Get(con, robaId);
            double stopa = 20;

            using (FbCommand cmd = new FbCommand("SELECT STOPA FROM TARIFE WHERE TARIFAID = @TARIFAID", con))
            {
                cmd.Parameters.AddWithValue("@TARIFAID", roba.TarifaID);
                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        stopa = Convert.ToDouble(dr["STOPA"]);
            }

            using (FbCommand cmd = new FbCommand(@"INSERT INTO STAVKA (VRDOK, BRDOK, MAGACINID, ROBAID, VRSTA, NAZIV, NABCENSAPOR, FAKTURNACENA, NABCENABT,
                                        TROSKOVI, NABAVNACENA, PRODCENABP, KOREKCIJA, PRODAJNACENA, DEVIZNACENA, DEVPRODCENA, KOLICINA,
                                        NIVKOL, TARIFAID, IMAPOREZ, POREZ, RABAT, MARZA, TAKSA, AKCIZA, PROSNAB, PRECENA, PRENAB, PROSPROD,
                                        MTID, PT, TREN_STANJE, POREZ_ULAZ, DEVNABCENA, POREZ_IZ)
                                        VALUES (@VRDOK, @BRDOK, @MAGACINID, @ROBAID, @VRSTA, @NAZIV, @NABCENSAPOR, @FAKTURNACENA, 0, 
                                        0, 0, @CENA, 0, @CENA, @DEVIZNACENA, 0, @KOL,
                                        0, @TARIFAID, @IMAPOREZ, @POREZ, @RABAT, 0, 0, 0, 0, 0, 0, 0, 
                                        @MTID, 'P', 0, @POREZ_ULAZ, @DEVNABCENA, @POREZ_IZ) RETURNING STAVKAID", con))
            {
                cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                cmd.Parameters.AddWithValue("@BRDOK", brDok);
                cmd.Parameters.AddWithValue("@MAGACINID", dokument.MagacinID);
                cmd.Parameters.AddWithValue("@NAZIV", roba.Naziv);
                cmd.Parameters.AddWithValue("@VRSTA", roba.Vrsta);
                cmd.Parameters.AddWithValue("@TARIFAID", roba.TarifaID);
                cmd.Parameters.AddWithValue("@IMAPOREZ", stopa > 0 ? 1 : 0);
                cmd.Parameters.AddWithValue("@POREZ", vrDok == 6 ? 0 : stopa);
                cmd.Parameters.AddWithValue("@POREZ_IZ", vrDok == 6 ? 0 : stopa);
                cmd.Parameters.AddWithValue("@POREZ_ULAZ", stopa);
                cmd.Parameters.AddWithValue("@MTID", dokument.MTID);
                cmd.Parameters.AddWithValue("@ROBAID", robaId);
                cmd.Parameters.AddWithValue("@CENA", vrDok == 6 ? 0 : cenaBezPdv);
                cmd.Parameters.AddWithValue("@KOL", kolicina);
                cmd.Parameters.AddWithValue("@RABAT", rabat);
                cmd.Parameters.AddWithValue("@NABCENSAPOR", vrDok == 6 ? cenaBezPdv * ((100 + stopa) / 100) : 0);
                cmd.Parameters.AddWithValue("@FAKTURNACENA", vrDok == 6 ? cenaBezPdv : 0);
                cmd.Parameters.AddWithValue("@DEVNABCENA", 0);
                cmd.Parameters.AddWithValue("@DEVCENA", 0);
                cmd.Parameters.AddWithValue("@DEVIZNACENA", vrDok == 6 ? cenaBezPdv : 0);

                cmd.Parameters.Add(new FbParameter { ParameterName = "STAVKAID", FbDbType = FbDbType.Integer, Direction = System.Data.ParameterDirection.Output });

                cmd.ExecuteNonQuery();

                return Convert.ToInt32(cmd.Parameters["STAVKAID"].Value);
            }
        }
    }
}
