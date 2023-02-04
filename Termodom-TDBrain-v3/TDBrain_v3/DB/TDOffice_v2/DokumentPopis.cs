using FirebirdSql.Data.FirebirdClient;

namespace TDBrain_v3.DB.TDOffice_v2
{
    /// <summary>
    /// Klasa za komunikaciju sa tabelom DokumentPopis
    /// </summary>
    public class DokumentPopis
    {
        /// <summary>
        /// Vraca dokument iz baze
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Termodom.Data.Entities.TDOffice_v2.Popis Get(int id)
        {
            using (FbConnection con = new FbConnection(Settings.ConnectionStringTDOffice_v2.ConnectionString()))
            {
                con.Open();
                return Get(con, id);
            }
        }
        /// <summary>
        /// Vraca dokument iz baze
        /// </summary>
        /// <param name="con"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Termodom.Data.Entities.TDOffice_v2.Popis Get(FbConnection con, int id)
        {
            using (FbCommand cmd = new FbCommand(@"SELECT ID, DATUM, USERID, MAGACINID, STATUS,
                    KOMENTAR, INTERNI_KOMENTAR, TIP, KOMERCIJALNO_POPIS_BRDOK, KOMERCIJALNO_NARUDZBENICA_BRDOK,
                    SPECIJAL_STAMPA, ZADUZENJE_BRDOK_KOMERCIJALNO, ZABELEZENO_STANJE_POPISA, NAPOMENA FROM DOKUMENT_POPIS WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new Termodom.Data.Entities.TDOffice_v2.Popis()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Datum = Convert.ToDateTime(dr["DATUM"]),
                            UserID = Convert.ToInt32(dr["USERID"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            Status = (Termodom.Data.Enumerators.PopisStatus)Convert.ToInt32(dr["STATUS"]),
                            Komentar = dr["KOMENTAR"] is DBNull ? "" : dr["KOMENTAR"].ToString(),
                            InterniKomentar = dr["INTERNI_KOMENTAR"] is DBNull ? "" : dr["INTERNI_KOMENTAR"].ToString(),
                            Tip = (Termodom.Data.Enumerators.PopisTip)Convert.ToInt32(dr["TIP"]),
                            KomercijalnoPopisBrDok = dr["KOMERCIJALNO_POPIS_BRDOK"] is DBNull ? (int?)null : Convert.ToInt32(dr["KOMERCIJALNO_POPIS_BRDOK"]),
                            KomercijalnoNarudzbenicaBrDok = dr["KOMERCIJALNO_NARUDZBENICA_BRDOK"] is DBNull ? (int?)null : Convert.ToInt32(dr["KOMERCIJALNO_NARUDZBENICA_BRDOK"]),
                            SpecijalStampa = Convert.ToInt32(dr["SPECIJAL_STAMPA"]),
                            ZaduzenjeBrDokKomercijalno = dr["ZADUZENJE_BRDOK_KOMERCIJALNO"] is DBNull ? null : (int?)Convert.ToInt32(dr["ZADUZENJE_BRDOK_KOMERCIJALNO"]),
                            Napomena = dr["NAPOMENA"].ToStringOrDefault()
                        };

            }

            return null;
        }
        /// <summary>
        /// Vraca read only dictionary dokumenata iz baze
        /// </summary>
        /// <param name="whereQuery"></param>
        /// <returns></returns>
        public static Termodom.Data.Entities.TDOffice_v2.PopisDictionary Dictionary(string? whereQuery = null)
        {
            using(FbConnection con = new FbConnection(Settings.ConnectionStringTDOffice_v2.ConnectionString()))
            {
                con.Open();
                return Dictionary(con, whereQuery);
            }
        }
        /// <summary>
        /// Vraca read only dictionary dokumenata iz baze
        /// </summary>
        /// <returns></returns>
        public static Termodom.Data.Entities.TDOffice_v2.PopisDictionary Dictionary(FbConnection con, string? whereQuery = null)
        {
            if (!string.IsNullOrWhiteSpace(whereQuery))
                whereQuery = "AND " + whereQuery;

            Dictionary<int, Termodom.Data.Entities.TDOffice_v2.Popis> dict = new Dictionary<int, Termodom.Data.Entities.TDOffice_v2.Popis>();

            using (FbCommand cmd = new FbCommand(@"SELECT
ID, DATUM, USERID, MAGACINID, STATUS, KOMENTAR, INTERNI_KOMENTAR, TIP, KOMERCIJALNO_POPIS_BRDOK,
KOMERCIJALNO_NARUDZBENICA_BRDOK, SPECIJAL_STAMPA, ZADUZENJE_BRDOK_KOMERCIJALNO, ZABELEZENO_STANJE_POPISA,
NAPOMENA
FROM DOKUMENT_POPIS
WHERE 1 = 1 " + whereQuery, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        dict.Add(Convert.ToInt32(dr["ID"]), new Termodom.Data.Entities.TDOffice_v2.Popis()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Datum = Convert.ToDateTime(dr["DATUM"]),
                            UserID = Convert.ToInt32(dr["USERID"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            Status = (Termodom.Data.Enumerators.PopisStatus)Convert.ToInt32(dr["STATUS"]),
                            Komentar = dr["KOMENTAR"] is DBNull ? "" : dr["KOMENTAR"].ToString(),
                            InterniKomentar = dr["INTERNI_KOMENTAR"] is DBNull ? "" : dr["INTERNI_KOMENTAR"].ToString(),
                            Tip = (Termodom.Data.Enumerators.PopisTip)Convert.ToInt32(dr["TIP"]),
                            KomercijalnoPopisBrDok = dr["KOMERCIJALNO_POPIS_BRDOK"] is DBNull ? (int?)null : Convert.ToInt32(dr["KOMERCIJALNO_POPIS_BRDOK"]),
                            KomercijalnoNarudzbenicaBrDok = dr["KOMERCIJALNO_NARUDZBENICA_BRDOK"] is DBNull ? (int?)null : Convert.ToInt32(dr["KOMERCIJALNO_NARUDZBENICA_BRDOK"]),
                            SpecijalStampa = Convert.ToInt32(dr["SPECIJAL_STAMPA"]),
                            ZaduzenjeBrDokKomercijalno = dr["ZADUZENJE_BRDOK_KOMERCIJALNO"] is DBNull ? null : (int?)Convert.ToInt32(dr["ZADUZENJE_BRDOK_KOMERCIJALNO"]),
                            Napomena = dr["NAPOMENA"].ToStringOrDefault()
                        });

            }

            return new Termodom.Data.Entities.TDOffice_v2.PopisDictionary(dict);
        }
        /// <summary>
        /// Dodaje novi dokument popisa u bazu i vraca id novokreiranog dokumenta
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="magacinID"></param>
        /// <param name="status"></param>
        /// <param name="komentar"></param>
        /// <param name="interniKomentar"></param>
        /// <param name="tip"></param>
        /// <param name="komercijalnoPopisBrDok"></param>
        /// <param name="komercijalnoNarudzbenicaBrDok"></param>
        /// <returns></returns>
        public static int Insert(int userID, int magacinID, int status, string komentar, string interniKomentar, Termodom.Data.Enumerators.PopisTip tip, int? komercijalnoPopisBrDok, int? komercijalnoNarudzbenicaBrDok)
        {
            using (FbConnection con = new FbConnection(Settings.ConnectionStringTDOffice_v2.ConnectionString()))
            {
                con.Open();
                return Insert(con, userID, magacinID, status, komentar, interniKomentar, tip, komercijalnoPopisBrDok, komercijalnoNarudzbenicaBrDok);
            }
        }
        /// <summary>
        /// Dodaje novi dokument popisa u bazu i vraca id novokreiranog dokumenta
        /// </summary>
        /// <param name="con"></param>
        /// <param name="userID"></param>
        /// <param name="magacinID"></param>
        /// <param name="status"></param>
        /// <param name="komentar"></param>
        /// <param name="interniKomentar"></param>
        /// <param name="tip"></param>
        /// <param name="komercijalnoPopisBrDok"></param>
        /// <param name="komercijalnoNarudzbenicaBrDok"></param>
        /// <returns></returns>
        public static int Insert(FbConnection con, int userID, int magacinID, int status, string komentar, string interniKomentar, Termodom.Data.Enumerators.PopisTip tip, int? komercijalnoPopisBrDok, int? komercijalnoNarudzbenicaBrDok)
        {
            using (FbCommand cmd = new FbCommand(@"INSERT INTO DOKUMENT_POPIS
                        (ID, DATUM, USERID, MAGACINID, STATUS, KOMENTAR, INTERNI_KOMENTAR, TIP, KOMERCIJALNO_POPIS_BRDOK, KOMERCIJALNO_NARUDZBENICA_BRDOK)
                        VALUES (((SELECT COALESCE(MAX(ID), 0) FROM DOKUMENT_POPIS) + 1), @D, @U, @M, @S, @K, @IK, @T, @KPB, @KNB)
                        RETURNING ID", con))
            {
                cmd.Parameters.AddWithValue("@D", DateTime.Now);
                cmd.Parameters.AddWithValue("@U", userID);
                cmd.Parameters.AddWithValue("@M", magacinID);
                cmd.Parameters.AddWithValue("@S", status);
                cmd.Parameters.AddWithValue("@K", komentar);
                cmd.Parameters.AddWithValue("@IK", interniKomentar);
                cmd.Parameters.AddWithValue("@T", (int)tip);
                cmd.Parameters.AddWithValue("@KPB", komercijalnoPopisBrDok);
                cmd.Parameters.AddWithValue("@KNB", komercijalnoNarudzbenicaBrDok);
                cmd.Parameters.Add(new FbParameter { ParameterName = "ID", FbDbType = FbDbType.Integer, Direction = System.Data.ParameterDirection.Output });

                cmd.ExecuteNonQuery();

                return Convert.ToInt32(cmd.Parameters["ID"].Value);
            }
        }
    }
}
