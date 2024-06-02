using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace TDOffice_v2.Komercijalno
{
    public static class Komercijalno
    {
        public static Dictionary<int, string> CONNECTION_STRING { get; set; } = new Dictionary<int, string>()
        {
            { 2021, "data source=4monitor; initial catalog = C:\\Poslovanje\\Baze\\2021\\FIRMA2021.FDB; user=SYSDBA; password=m; pooling=True" },
            { 2020, "data source=4monitor; initial catalog = C:\\Poslovanje\\Baze\\2020\\FIRMA2020.FDB; user=SYSDBA; password=m; pooling=True" },
            { 2019, "data source=4monitor; initial catalog = C:\\Poslovanje\\Baze\\2019\\FIRMA2019.FDB; user=SYSDBA; password=m; pooling=True" },
            { 2018, "data source=4monitor; initial catalog = C:\\Poslovanje\\Baze\\2018\\FIRMA2018.FDB; user=SYSDBA; password=m; pooling=True" },
            { 2017, "data source=4monitor; initial catalog = C:\\Poslovanje\\Baze\\2017\\FIRMA2017.FDB; user=SYSDBA; password=m; pooling=True" }
        };

        public static List<int> izlazniVrDok { get; set; } = new List<int>()
                {
                    15,
                    19,
                    17,
                    23,
                    13,
                    14,
                    27,
                    25,
                    28
                };
        public static List<int> ulazniVrDok { get; set; } = new List<int>()
                {
                    0,
                    11,
                    18,
                    16,
                    22,
                    1,
                    2,
                    26,
                    29,
                    30
                };
        /// <summary>
        /// Vraca realnu nabavnu cenu uzimajuci u obzir i cene koje nam je
        /// proizvodjac ponudio ali jos uvek nemamo dokument ulaza po tim cenama.
        /// </summary>
        /// <param name="robaID"></param>
        /// <param name="datum"></param>
        /// <param name="dokumentiNabavke"></param>
        /// <param name="stavkeNabavke"></param>
        /// <returns></returns>
        public static double GetRealnaNabavnaCena(int robaID, DateTime datum, List<Dokument> dokumentiNabavke, List<Stavka> stavkeNabavke)
        {
            stavkeNabavke = stavkeNabavke.Where(x => x.RobaID == robaID).ToList();
            List<Dokument> doks = new List<Dokument>();

            foreach (Stavka s in stavkeNabavke)
            {
                Dokument d = dokumentiNabavke.FirstOrDefault(x => x.VrDok == s.VrDok && x.BrDok == s.BrDok);
                if (d != null)
                    doks.Add(d);
            }

            dokumentiNabavke = doks;
                
            Dokument dokument36 = dokumentiNabavke.FirstOrDefault(x => x.VrDok == 36 && datum >= x.Datum && datum <= x.DatRoka);

            if (dokument36 != null)
                return stavkeNabavke.FirstOrDefault(x => x.VrDok == dokument36.VrDok && x.BrDok == dokument36.BrDok).NabavnaCena;

            List<Dokument> dokumentiKojiDolazeUObzir = new List<Dokument>(dokumentiNabavke);
            dokumentiKojiDolazeUObzir.RemoveAll(x => x.Datum > datum || !new int[] { 0, 1, 2, 3 }.Contains(x.VrDok));
            dokumentiKojiDolazeUObzir.Sort((y, x) => x.Datum.CompareTo(y.Datum));

            Dokument vazeciDokumentNabavke = dokumentiKojiDolazeUObzir.FirstOrDefault();

            if (vazeciDokumentNabavke == null)
                return 0;

            return stavkeNabavke.FirstOrDefault(x => x.VrDok == vazeciDokumentNabavke.VrDok && x.BrDok == vazeciDokumentNabavke.BrDok).NabavnaCena;
        }
//        public static double GetRealnaNabavnaCena1(FbConnection con, int robaID, DateTime odDatuma, DateTime doDatuma, bool ukljuciIDokument36 = true)
//        {
//            throw new Exception("Ne radi!");
//            double ret = 0;
//            DateTime maxVazeciDokumentDatum = DateTime.Now.AddYears(-10);
//            using (FbCommand cmd = new FbCommand(@"SELECT MAX(d.DATROKA) FROM STAVKA s LEFT OUTER JOIN DOKUMENT d on s.VRDOK = d.VRDOK AND s.BRDOK = d.BRDOK WHERE d.VRDOK IN (0, 1, 2) AND s.ROBAID = @ROBAID AND d.MAGACINID = 50", con))
//            {
//                cmd.Parameters.AddWithValue("@ROBAID", robaID);
//                using (FbDataReader dr = cmd.ExecuteReader())
//                    if (dr.Read())
//                        if(!(dr[0] is DBNull))
//                            maxVazeciDokumentDatum = Convert.ToDateTime(dr[0]);
//            }

//            string selectCommand = @"SELECT
//COALESCE(-1, NULL) AS DOK, t2.PNC AS PNC, t2.DATUM as DATUM, t2.DATROKA as DATROK
//FROM
//(
//    SELECT
//    COALESCE(s.NABAVNACENA, 0) AS PNC, t1.DATROKA, t1.DATUM
//    FROM
//    (
//        SELECT
//        FIRST 1 s.STAVKAID, d.DATROKA, d.DATUM
//        FROM DOKUMENT d
//        LEFT OUTER JOIN STAVKA s ON s.VRDOK = d.VRDOK AND s.BRDOK = d.BRDOK
//        WHERE
//        d.VRDOK IN (0, 1, 2)
//        AND d.DATROKA >= @DATUM_OD
//        AND d.DATROKA <= @DATUM_DO
//        AND s.ROBAID = @ROBAID
//        AND d.MAGACINID = 50
//        ORDER BY DATROKA DESC
//    ) AS t1
//    LEFT OUTER JOIN STAVKA s ON t1.STAVKAID = s.STAVKAID
//) AS t2
//UNION
//SELECT
//COALESCE(36, NULL) AS DOK, t3.PNC AS PNC, t3.DATUM as DATUM, t3.DATROKA as DATROK
//FROM
//(
//    SELECT
//    s.NABAVNACENA AS PNC, d.DATUM, MAX(t1.DATROKA) AS DATROKA
//    FROM
//    (
//        SELECT
//        FIRST 1 s.STAVKAID, d.DATROKA
//        FROM DOKUMENT d
//        LEFT OUTER JOIN STAVKA s ON s.VRDOK = d.VRDOK AND s.BRDOK = d.BRDOK
//        WHERE
//        d.VRDOK = " + (ukljuciIDokument36 ? "36" : "-4758125") + @"
//        AND d.DATROKA >= @DATUM_DO
//        AND s.ROBAID = @ROBAID
//        AND d.MAGACINID = 50
//        ORDER BY DATROKA ASC
//    ) AS t1
//    LEFT OUTER JOIN STAVKA s ON t1.STAVKAID = s.STAVKAID
//    LEFT OUTER JOIN DOKUMENT d on s.VRDOK = d.VRDOK AND s.BRDOK = d.BRDOK
//    GROUP BY NABAVNACENA, DATUM
//) AS t3";

//            using (FbCommand cmd = new FbCommand(selectCommand, con))
//            {
//                cmd.Parameters.AddWithValue("@ROBAID", robaID);
//                cmd.Parameters.AddWithValue("@DATUM_OD", odDatuma);
//                cmd.Parameters.AddWithValue("@DATUM_DO", doDatuma);

//                DataTable dt = new DataTable();
//                using (FbDataAdapter da = new FbDataAdapter(cmd))
//                    da.Fill(dt);

//                DataRow row36 = null;
//                DataRow rowDok = null;

//                // Mora uhvatiti barem jedan dokument koji diktira cenu
//                try
//                {
//                    row36 = dt.Select("DOK = 36")[0];
//                }
//                catch
//                {

//                }
//                try { rowDok = dt.Select("DOK <> 36")[0]; }
//                catch
//                {

//                }

//                if (row36 == null && rowDok == null)
//                    return 0;

//                if (row36 == null)
//                    return Convert.ToDouble(rowDok[1]);
//                else if (rowDok == null)
//                    return Convert.ToDouble(row36[1]);

//                DateTime dateBaseDokument = Convert.ToDateTime(rowDok[2]);
//                DateTime dok36Od = Convert.ToDateTime(row36[2]);
//                DateTime dok36Do = Convert.ToDateTime(row36[3]);

//                if (dateBaseDokument.Date == maxVazeciDokumentDatum.Date)
//                    ret = doDatuma < dok36Od ? Convert.ToDouble(rowDok[1]) : Convert.ToDouble(row36[1]);
//                else
//                    ret = dateBaseDokument > dok36Od && dateBaseDokument < dok36Do ? Convert.ToDouble(row36[1]) : Convert.ToDouble(rowDok[1]);
//            }
//            return ret;
//        }
        public static double GetProsecnaNabavnaCena(FbConnection con, int robaID, DateTime odDatuma, DateTime doDatuma)
        {
            using (FbCommand cmd = new FbCommand(@"
SELECT
(SUM(s.KOLICINA * s.NABAVNACENA) / SUM(s.KOLICINA)) as PROSECNA_NABAVNA_CENA
FROM DOKUMENT d
LEFT OUTER JOIN STAVKA s ON s.VRDOK = d.VRDOK AND s.BRDOK = d.BRDOK
WHERE
d.VRDOK IN (0, 1, 2, 36)
AND d.DATUM >= @DATUM_OD
AND d.DATUM <= @DATUM_DO
AND d.MAGACINID = 50
AND s.ROBAID = @ROBAID", con))
            {
                cmd.Parameters.AddWithValue("@ROBAID", robaID);
                cmd.Parameters.AddWithValue("@DATUM_OD", odDatuma);
                cmd.Parameters.AddWithValue("@DATUM_DO", doDatuma);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return dr[0] is DBNull ? 0 : Convert.ToDouble(dr[0]);
            }
            return 0;
        }
    }
}
