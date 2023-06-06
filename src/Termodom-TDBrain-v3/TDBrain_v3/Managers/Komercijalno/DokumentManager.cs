using FirebirdSql.Data.FirebirdClient;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.Managers.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    public class DokumentManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="con"></param>
        /// <param name="vrDok"></param>
        /// <param name="brDok"></param>
        /// <returns></returns>
        public static Dokument? Get(FbConnection con, int vrDok, int brDok)
        {
            using(FbCommand cmd = new FbCommand("SELECT * FROM DOKUMENT WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK", con))
            {
                cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                cmd.Parameters.AddWithValue("@BRDOK", brDok);

                using(FbDataReader dr = cmd.ExecuteReader())
                {
                    if(dr.Read())
                    {
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
                }

                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="con"></param>
        /// <param name="vrDok"></param>
        /// <param name="magacinId"></param>
        /// <param name="interniBroj"></param>
        /// <param name="ppid"></param>
        /// <param name="nuid"></param>
        /// <param name="komercijalnoKorisnikId"></param>
        /// <param name="dozvoliDaljeIzmeneUKomercijalnom"></param>
        /// <returns></returns>
        public static int Insert(FbConnection con, int vrDok, int magacinId, string? interniBroj, int? ppid, int nuid, int? komercijalnoKorisnikId, bool dozvoliDaljeIzmeneUKomercijalnom = true)
        {
            int poslednjiBrDok = 0;
            using (FbCommand cmd = new FbCommand(@"SELECT POSLEDNJI FROM VRSTADOKMAG WHERE VRDOK = @VRDOK AND MAGACINID = @MAGACINID", con))
            {
                cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                cmd.Parameters.AddWithValue("@MAGACINID", magacinId);

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
                cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                cmd.Parameters.AddWithValue("@MAGID", magacinId);
                cmd.Parameters.AddWithValue("@BRDOK", poslednjiBrDok + 1);
                cmd.Parameters.AddWithValue("@INTERNIBROJ", interniBroj);
                cmd.Parameters.AddWithValue("@DATUM", DateTime.Now);
                cmd.Parameters.AddWithValue("@LINKED", ProceduraManager.NextLinked(con, DateTime.Now, magacinId));
                cmd.Parameters.AddWithValue("@MAGACINID", magacinId);
                cmd.Parameters.AddWithValue("@MTID", MagacinManager.Get(con, magacinId)?.MTID);
                cmd.Parameters.AddWithValue("@PPID", ppid);
                cmd.Parameters.AddWithValue("@NUID", nuid);
                cmd.Parameters.AddWithValue("@ZAPID", komercijalnoKorisnikId ?? 1);
                cmd.Parameters.AddWithValue("@PREVOZROBE", ppid == null ? 0 : 1);
                cmd.Parameters.AddWithValue("@VRDOKIN", dozvoliDaljeIzmeneUKomercijalnom ? (int?)null : 8);

                cmd.ExecuteNonQuery();

                return poslednjiBrDok + 1;
            }
        }
    }
}
