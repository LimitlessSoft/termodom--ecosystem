using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using Termodom.Data.Entities.TDOffice_v2;
using static Termodom.Data.Entities.TDOffice_v2.FiskalniRacun;

namespace TDBrain_v3.Managers.TDOffice_v2
{
    public static class FiskalniRacunManager
    {
        public static void Insert(FiskalniRacun fiskalniRacun)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringTDOffice_v2.ConnectionString()))
            {
                con.Open();
                Insert(con, fiskalniRacun);
            }
        }
        public static FiskalniRacunDictionary Dictionary(List<string> whereParameters = null)
        {
            using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringTDOffice_v2.ConnectionString()))
            {
                con.Open();
                return Dictionary(con, whereParameters);
            }
        }

        public static void Insert(FbConnection con, FiskalniRacun fiskalniRacun)
        {
            using (FbCommand cmd = new FbCommand(@"INSERT INTO FISKALNI_RACUN
(
    INVOICE_NUMBER,
    TIN,
    REQUESTED_BY,
    DATE_AND_TIME_OF_POS,
    CASHIER,
    BUYER_TIN,
    BUYERS_COST_CENTER,
    POS_INVOICE_NUMBER,
    SDCTIME_SERVER_TIME_ZONE,
    INVOICE_COUNTER,
    SIGNED_BY,
    TOTAL_AMOUNT,
    TRANSACTION_TYPE,
    INVOICE_TYPE
)
VALUES
(
    @INVOICE_NUMBER,
    @TIN,
    @REQUESTED_BY,
    @DATE_AND_TIME_OF_POS,
    @CASHIER,
    @BUYER_TIN,
    @BUYERS_COST_CENTER,
    @POS_INVOICE_NUMBER,
    @SDCTIME_SERVER_TIME_ZONE,
    @INVOICE_COUNTER,
    @SIGNED_BY,
    @TOTAL_AMOUNT,
    @TRANSACTION_TYPE,
    @INVOICE_TYPE
)", con))
            {
                cmd.Parameters.AddWithValue("@INVOICE_NUMBER", fiskalniRacun.InvoiceNumber);
                cmd.Parameters.AddWithValue("@TIN", fiskalniRacun.TIN);
                cmd.Parameters.AddWithValue("@REQUESTED_BY", fiskalniRacun.RequestedBy);
                cmd.Parameters.AddWithValue("@DATE_AND_TIME_OF_POS", fiskalniRacun.DateAndTimeOfPos);
                cmd.Parameters.AddWithValue("@CASHIER", fiskalniRacun.Cashier);
                cmd.Parameters.AddWithValue("@BUYER_TIN", fiskalniRacun.BuyerTin);
                cmd.Parameters.AddWithValue("@BUYERS_COST_CENTER", fiskalniRacun.BuyersCostCenter);
                cmd.Parameters.AddWithValue("@POS_INVOICE_NUMBER", fiskalniRacun.PosInvoiceNumber);
                cmd.Parameters.AddWithValue("@SDCTIME_SERVER_TIME_ZONE", fiskalniRacun.SDCTime_ServerTimeZone);
                cmd.Parameters.AddWithValue("@INVOICE_COUNTER", fiskalniRacun.InvoiceCounter);
                cmd.Parameters.AddWithValue("@SIGNED_BY", fiskalniRacun.SignedBy);
                cmd.Parameters.AddWithValue("@TOTAL_AMOUNT", fiskalniRacun.TotalAmount);
                Encoding enc = Encoding.GetEncoding(855);
                cmd.Parameters.AddWithValue("@INVOICE_TYPE", enc.GetBytes(fiskalniRacun.InvoiceType));
                cmd.Parameters.AddWithValue("@TRANSACTION_TYPE", enc.GetBytes(fiskalniRacun.TransactionType));

                cmd.ExecuteNonQuery();
            }
        }
        public static FiskalniRacunDictionary Dictionary(FbConnection con, List<string> whereParameters = null)
        {
            string whereQuery = "";

            if (whereParameters != null && whereParameters.Count > 0)
                whereQuery = " WHERE " + string.Join(" AND ", whereParameters);

            Dictionary<string, FiskalniRacun> dict = new Dictionary<string, FiskalniRacun>();
            using(FbCommand cmd = new FbCommand("SELECT * FROM FISKALNI_RACUN " + whereQuery, con))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding enc = Encoding.GetEncoding(855);
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                    {
                        dict.Add(dr["INVOICE_NUMBER"].ToString(), new FiskalniRacun()
                        {
                            InvoiceNumber = dr["INVOICE_NUMBER"].ToString(),
                            TIN = dr["TIN"].ToString(),
                            RequestedBy = dr["REQUESTED_BY"].ToString(),
                            DateAndTimeOfPos = dr["DATE_AND_TIME_OF_POS"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["DATE_AND_TIME_OF_POS"], new System.Globalization.CultureInfo("de-DE")),
                            Cashier = dr["CASHIER"].ToString(),
                            BuyerTin = dr["BUYER_TIN"].ToStringOrDefault(),
                            BuyersCostCenter = dr["BUYERS_COST_CENTER"].ToStringOrDefault(),
                            PosInvoiceNumber = dr["POS_INVOICE_NUMBER"].ToString(),
                            PaymentMethod = dr["PAYMENT_METHOD"].ToStringOrDefault(),
                            SDCTime_ServerTimeZone = Convert.ToDateTime(dr["SDCTIME_SERVER_TIME_ZONE"]),
                            InvoiceCounter = dr["INVOICE_COUNTER"].ToString(),
                            SignedBy = dr["SIGNED_BY"].ToString(),
                            TotalAmount = Convert.ToDouble(dr["TOTAL_AMOUNT"]),
                            TransactionType = enc.GetString((byte[])dr["TRANSACTION_TYPE"]),
                            InvoiceType = enc.GetString((byte[])dr["INVOICE_TYPE"])
                        });
                    }
            }
            return new FiskalniRacunDictionary(dict);
        }
    }
}
