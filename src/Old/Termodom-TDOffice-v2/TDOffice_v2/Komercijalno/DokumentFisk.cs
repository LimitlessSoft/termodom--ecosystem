using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.Komercijalno
{
    public class DokumentFisk
    {
        public int DFID { get; set; }
        public int VrDok { get; set; }
        public int BrDok { get; set; }
        public int TransactionType { get; set; }
        public int InvoiceType { get; set; }
        public string ReferentDocumentNumber { get; set; }
        public string ReferentDocumentDT { get; set; }
        public DateTime Poslat { get; set; }
        public int ZapID { get; set; }
        public int Status { get; set; }
        public string Napomena { get; set; }
        public string LinkedReferentDocumentNumber { get; set; }
        public string LinkedReferentDocumentDT { get; set; }
        public string ByerID { get; set; }
        public string ByerOP { get; set; }

        public static DokumentFisk Get(int godina, int dfid)
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return Get(con, dfid);
            }
        }
        public static DokumentFisk Get(FbConnection con, int dfid)
        {
            using(FbCommand cmd = new FbCommand("SELECT DFID, VRDOK, BRDOK, TRANSACTIONTYPE, INVOICETYPE, REFERENTDOCUMENTNUMBER, REFERENTDOCUMENTDT, POSLAT, ZAPID, STATUS, NAPOMENA, LINKED_REFERENTDOCUMENTNUMBER, LINKED_REFERENTDOCUMENTDT, BYERID, BYEROP" +
                " FROM DOKUMENT_FISK" +
                " WHERE DFID = @DFID", con))
            {
                cmd.Parameters.AddWithValue("@DFID", dfid);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new DokumentFisk()
                        {
                            DFID = Convert.ToInt32(dr["DFID"]),
                            VrDok = Convert.ToInt32(dr["VRDOK"]),
                            BrDok = Convert.ToInt32(dr["BRDOK"]),
                            TransactionType = Convert.ToInt32(dr["TRANSACTIONTYPE"]),
                            InvoiceType = Convert.ToInt32(dr["INVOICETYPE"]),
                            ReferentDocumentNumber = dr["REFERENTDOCUMENTNUMBER"].ToStringOrDefault(),
                            ReferentDocumentDT = dr["REFERENTDOCUMENTDT"].ToStringOrDefault(),
                            Poslat = Convert.ToDateTime(dr["POSLAT"]),
                            ZapID = Convert.ToInt32(dr["ZAPID"]),
                            Status = Convert.ToInt32(dr["STATUS"]),
                            Napomena = dr["NAPOMENA"].ToStringOrDefault(),
                            LinkedReferentDocumentNumber = dr["LINKED_REFERENTDOCUMENTNUMBER"].ToStringOrDefault(),
                            LinkedReferentDocumentDT = dr["LINKED_REFERENTDOCUMENTDT"].ToStringOrDefault(),
                            ByerID = dr["BYERID"].ToStringOrDefault(),
                            ByerOP = dr["BYEROP"].ToStringOrDefault()
                        };
            }

            return null;
        }

        public static List<DokumentFisk> List(int godina, string whereQuery = null)
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return List(con, whereQuery);
            }
        }
        public static List<DokumentFisk> List(FbConnection con, string whereQuery = null)
        {
            if (!string.IsNullOrWhiteSpace(whereQuery))
                whereQuery = " WHERE " + whereQuery;

            List<DokumentFisk> list = new List<DokumentFisk>();

            using (FbCommand cmd = new FbCommand("SELECT DFID, VRDOK, BRDOK, TRANSACTIONTYPE, INVOICETYPE, REFERENTDOCUMENTNUMBER, REFERENTDOCUMENTDT, POSLAT, ZAPID, STATUS, NAPOMENA, LINKED_REFERENTDOCUMENTNUMBER, LINKED_REFERENTDOCUMENTDT, BYERID, BYEROP" +
                " FROM DOKUMENT_FISK" + whereQuery, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                    {
                        list.Add(new DokumentFisk()
                        {
                            DFID = Convert.ToInt32(dr["DFID"]),
                            VrDok = Convert.ToInt32(dr["VRDOK"]),
                            BrDok = Convert.ToInt32(dr["BRDOK"]),
                            TransactionType = Convert.ToInt32(dr["TRANSACTIONTYPE"]),
                            InvoiceType = Convert.ToInt32(dr["INVOICETYPE"]),
                            ReferentDocumentNumber = dr["REFERENTDOCUMENTNUMBER"].ToStringOrDefault(),
                            ReferentDocumentDT = dr["REFERENTDOCUMENTDT"].ToStringOrDefault(),
                            Poslat = Convert.ToDateTime(dr["POSLAT"]),
                            ZapID = Convert.ToInt32(dr["ZAPID"]),
                            Status = Convert.ToInt32(dr["STATUS"]),
                            Napomena = dr["NAPOMENA"].ToStringOrDefault(),
                            LinkedReferentDocumentNumber = dr["LINKED_REFERENTDOCUMENTNUMBER"].ToStringOrDefault(),
                            LinkedReferentDocumentDT = dr["LINKED_REFERENTDOCUMENTDT"].ToStringOrDefault(),
                            ByerID = dr["BYERID"].ToStringOrDefault(),
                            ByerOP = dr["BYEROP"].ToStringOrDefault()
                        });
                    }
            }
            return list;
        }
        public static Task<List<DokumentFisk>> ListAsync(int godina, string whereQuery = null)
        {
            return Task.Run(() =>
            {
                return List(godina, whereQuery);
            });
        }
    }
}
