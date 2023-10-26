using FirebirdSql.Data.FirebirdClient;
using System.Text;
using TDBrain_v3.RequestBodies.TDOffice;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDBrain_v3.Managers.TDOffice_v2
{
    /// <summary>
    /// Klasa za komunikaciju sa tabelom korisnika
    /// </summary>
    public static class KorisnikManager
    {
        /// <summary>
        /// Kreira novog korisnika u TDOffice-u
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static int Create(KorisnikInsertRequestBody body)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringTDOffice_v2.ConnectionString()))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand(@"
                    INSERT INTO KORISNIK
                    (
                        ID,
                        KORISNICKO_IME,
                        SIFRA,
                        MAGACINID,
                        KOMERCIJALNO_USER_ID,
                        GRAD
                    )
                    VALUES
                    (
                        ((SELECT COALESCE(MAX(ID), 0) FROM KORISNIK) + 1),
                        @KORISNICKO_IME,
                        @SIFRA,
                        @MAGACINID,
                        @KOMERCIJALNO_USER_ID,
                        @GRAD
                    )
                    RETURNING
                    ID", con))
                {
                    cmd.Parameters.Add(new FbParameter("ID", FbDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                    cmd.Parameters.AddWithValue("@KORISNICKO_IME", body.KorisnickoIme);
                    cmd.Parameters.AddWithValue("@SIFRA", body.Sifra);
                    cmd.Parameters.AddWithValue("@MAGACINID", body.MagacinId);
                    cmd.Parameters.AddWithValue("@KOMERCIJALNO_USER_ID", body.KomercijalnoUserId);
                    cmd.Parameters.AddWithValue("@GRAD", body.Grad);

                    if (cmd.ExecuteNonQuery() > 0)
                        return Convert.ToInt32(cmd.Parameters["ID"].Value);

                    return -1;
                }
            }
        }
        /// <summary>
        /// Vraca dictinary beleski korisnika
        /// </summary>
        /// <param name="korisnikID"></param>
        /// <returns></returns>
        public static Korisnik.BeleskaDictionary BeleskeDictionary(int korisnikID)
        {
            Dictionary<int, Korisnik.Beleska> dict = new Dictionary<int, Korisnik.Beleska>();
            using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringTDOffice_v2.ConnectionString()))
            {
                con.Open();
                using(FbCommand cmd = new FbCommand("SELECT * FROM KORISNIK_BELESKA WHERE KORISNIK_ID = @KORISNIK_ID", con))
                {
                    cmd.Parameters.AddWithValue("@KORISNIK_ID", korisnikID);

                    using (FbDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            dict.Add(Convert.ToInt32(dr["ID"]), new Korisnik.Beleska()
                            {
                                Id = Convert.ToInt32(dr["ID"]),
                                KorisnikId = korisnikID,
                                Naslov = dr["NASLOV"].ToString(),
                                Body = dr["BODY"] is DBNull ? "" : Encoding.UTF8.GetString((byte[])dr["BODY"])
                            });

                }
            }
            return new Korisnik.BeleskaDictionary(dict);
        }

        public static int BeleskaInsert(KorisnikBeleskaSaveRequestBody body)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringTDOffice_v2.ConnectionString()))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand(@"
                    INSERT INTO KORISNIK_BELESKA
                    (
                        ID,
                        KORISNIK_ID,
                        NASLOV,
                        BODY
                    )
                    VALUES
                    (
                        ((SELECT COALESCE(MAX(ID), 0) FROM KORISNIK_BELESKA) + 1),
                        @KORISNIKID,
                        @NASLOV,
                        @BODY
                    )
                    RETURNING
                    ID", con))
                {
                    cmd.Parameters.Add(new FbParameter("ID", FbDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                    cmd.Parameters.AddWithValue("@KORISNIKID", body.KorisnikId);
                    cmd.Parameters.AddWithValue("@NASLOV", body.Naslov);
                    cmd.Parameters.AddWithValue("@BODY", Encoding.UTF8.GetBytes(body.Body));

                    cmd.ExecuteNonQuery();

                    return Convert.ToInt32(cmd.Parameters["ID"].Value);
                }
            }
        }
        public static int BeleskaUpdate(KorisnikBeleskaSaveRequestBody body)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringTDOffice_v2.ConnectionString()))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand(@"
                    UPDATE KORISNIK_BELESKA
                    SET
                        KORISNIK_ID = @KORISNIK_ID,
                        NASLOV = @NASLOV,
                        BODY = @BODY
                    WHERE
                        ID = @ID
                    ", con))
                {
                    cmd.Parameters.AddWithValue("@ID", body.Id);
                    cmd.Parameters.AddWithValue("@KORISNIKID", body.KorisnikId);
                    cmd.Parameters.AddWithValue("@NASLOV", body.Naslov);
                    cmd.Parameters.AddWithValue("@BODY", Encoding.UTF8.GetBytes(body.Body));

                    return -1;
                }
            }
        }
    }
}
