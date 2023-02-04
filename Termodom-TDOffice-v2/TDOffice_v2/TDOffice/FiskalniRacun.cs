using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public class FiskalniRacun
    {
        public string InvoiceNumber { get; set; }
        public string TIN { get; set; }
        public string RequestedBy { get; set; }
        public DateTime? DateAndTimeOfPos { get; set; }
        public string Cashier { get; set; }
        public object BuyerTin { get; set; }
        public object BuyersCostCenter { get; set; }
        public string PosInvoiceNumber { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime SDCTime_ServerTimeZone { get; set; }
        public string InvoiceCounter { get; set; }
        public string SignedBy { get; set; }
        public double TotalAmount { get; set; }
        public string TransactionType { get; set; }
        public string InvoiceType { get; set; }

        public static FiskalniRacun Get(string invoiceNumber)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Get(con, invoiceNumber);
            }
        }
        public static FiskalniRacun Get(FbConnection con, string invoiceNumber)
        {
            using(FbCommand cmd = new FbCommand(@"SELECT
                INVOICE_NUMBER,
                TIN,
                REQUESTED_BY,
                DATE_AND_TIME_OF_POS,
                CASHIER,
                BUYER_TIN,
                BUYERS_COST_CENTER,
                POS_INVOICE_NUMBER,
                PAYMENT_METHOD,
                SDCTIME_SERVER_TIME_ZONE,
                INVOICE_COUNTER,
                SIGNED_BY,
                TOTAL_AMOUNT,
                TRANSACTION_TYPE,
                INVOICE_TYPE
                FROM FISKALNI_RACUN
                WHERE INVOICE_NUMBER = @IN", con))
            {
                cmd.Parameters.AddWithValue("@IN", invoiceNumber);
                Encoding enc = Encoding.GetEncoding(855);
                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new FiskalniRacun()
                        {
                            InvoiceNumber = dr["INVOICE_NUMBER"].ToString(),
                            TIN = dr["TIN"].ToString(),
                            RequestedBy = dr["REQUESTED_BY"].ToString(),
                            DateAndTimeOfPos = dr["DATE_AND_TIME_OF_POS"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["DATE_AND_TIME_OF_POS"]),
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
                        };
            }
            return null;
        }

        public static List<FiskalniRacun> List(string whereQuery = null)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return List(con, whereQuery);
            }
        }
        public static List<FiskalniRacun> List(FbConnection con, string whereQuery = null)
        {
            if (!string.IsNullOrWhiteSpace(whereQuery))
                whereQuery = " WHERE " + whereQuery;

            List<FiskalniRacun> list = new List<FiskalniRacun>();

            using (FbCommand cmd = new FbCommand(@"SELECT
                INVOICE_NUMBER,
                TIN,
                REQUESTED_BY,
                DATE_AND_TIME_OF_POS,
                CASHIER,
                BUYER_TIN,
                BUYERS_COST_CENTER,
                POS_INVOICE_NUMBER,
                PAYMENT_METHOD,
                SDCTIME_SERVER_TIME_ZONE,
                INVOICE_COUNTER,
                SIGNED_BY,
                TOTAL_AMOUNT,
                TRANSACTION_TYPE,
                INVOICE_TYPE
                FROM FISKALNI_RACUN" + whereQuery, con))
            {
                Encoding enc = Encoding.GetEncoding(855);
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                    {
                        list.Add(new FiskalniRacun()
                        {
                            InvoiceNumber = dr["INVOICE_NUMBER"].ToString(),
                            TIN = dr["TIN"].ToString(),
                            RequestedBy = dr["REQUESTED_BY"].ToString(),
                            DateAndTimeOfPos = dr["DATE_AND_TIME_OF_POS"] is DBNull ?null :(DateTime?)Convert.ToDateTime(dr["DATE_AND_TIME_OF_POS"], new System.Globalization.CultureInfo("de-DE")),
                            //DateAndTimeOfPos = dr["DATE_AND_TIME_OF_POS"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["DATE_AND_TIME_OF_POS"]),
                            Cashier = dr["CASHIER"].ToString(),
                            BuyerTin = dr["BUYER_TIN"].ToStringOrDefault(),
                            BuyersCostCenter = dr["BUYERS_COST_CENTER"].ToStringOrDefault(),
                            PosInvoiceNumber = dr["POS_INVOICE_NUMBER"].ToString(),
                            PaymentMethod = dr["PAYMENT_METHOD"].ToStringOrDefault(),
                            SDCTime_ServerTimeZone = Convert.ToDateTime(dr["SDCTIME_SERVER_TIME_ZONE"]),
                            InvoiceCounter = dr["INVOICE_COUNTER"].ToString(),
                            SignedBy = dr["SIGNED_BY"].ToString(),
                            TotalAmount = Convert.ToDouble(dr["TOTAL_AMOUNT"]),
                            //TransactionType = dr["TRANSACTION_TYPE"].ToString(),
                            TransactionType = enc.GetString((byte[])dr["TRANSACTION_TYPE"]),
                            //InvoiceType = dr["INVOICE_TYPE"].ToString()
                            InvoiceType = enc.GetString((byte[])dr["INVOICE_TYPE"])
                        }) ;
                    }
            }
            return list;
        }

        public static void Insert(string invoiceNumber, string tin, string requestedBy, DateTime? dateAndTimeOfPos, string cashier, string buyerTin, string buyersCostCenter, string posInvoiceNumber,
            string paymentMethod, DateTime sDCTimeServerTimeZone, string invoiceCounter, string signedBy, double totalAmount, string transactionType, string invoiceType)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Insert(invoiceNumber, tin, requestedBy, dateAndTimeOfPos, cashier, buyerTin, buyersCostCenter, posInvoiceNumber,
                    paymentMethod, sDCTimeServerTimeZone, invoiceCounter, signedBy, totalAmount, transactionType, invoiceType);            }    
        }
        public static void Insert(FbConnection con, string invoiceNumber, string tin, string requestedBy, DateTime? dateAndTimeOfPos, string cashier, string buyerTin, string buyersCostCenter, string posInvoiceNumber,
            string paymentMethod, DateTime sDCTimeServerTimeZone, string invoiceCounter, string signedBy, double totalAmount, string transactionType, string invoiceType)
        {
            
            using(FbCommand cmd = new FbCommand(@"INSERT INTO FISKALNI_RACUN
(
    INVOICE_NUMBER,
    TIN,
    REQUESTED_BY,
    DATE_AND_TIME_OF_POS,
    CASHIER,
    BUYER_TIN,
    BUYERS_COST_CENTER,
    POS_INVOICE_NUMBER,
    PAYMENT_METHOD,
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
    @PAYMENT_METHOD,
    @SDCTIME_SERVER_TIME_ZONE,
    @INVOICE_COUNTER,
    @SIGNED_BY,
    @TOTAL_AMOUNT,
    @TRANSACTION_TYPE,
    @INVOICE_TYPE
)", con))
            {
                cmd.Parameters.AddWithValue("@INVOICE_NUMBER", invoiceNumber);
                cmd.Parameters.AddWithValue("@TIN", tin);
                cmd.Parameters.AddWithValue("@REQUESTED_BY", requestedBy);
                cmd.Parameters.AddWithValue("@DATE_AND_TIME_OF_POS", dateAndTimeOfPos);
                cmd.Parameters.AddWithValue("@CASHIER", cashier);
                cmd.Parameters.AddWithValue("@BUYER_TIN", buyerTin);
                cmd.Parameters.AddWithValue("@BUYERS_COST_CENTER", buyersCostCenter);
                cmd.Parameters.AddWithValue("@POS_INVOICE_NUMBER", posInvoiceNumber);
                cmd.Parameters.AddWithValue("@PAYMENT_METHOD", paymentMethod);
                cmd.Parameters.AddWithValue("@SDCTIME_SERVER_TIME_ZONE", sDCTimeServerTimeZone);
                cmd.Parameters.AddWithValue("@INVOICE_COUNTER", invoiceCounter);
                cmd.Parameters.AddWithValue("@SIGNED_BY", signedBy);
                cmd.Parameters.AddWithValue("@TOTAL_AMOUNT", totalAmount);
                //cmd.Parameters.AddWithValue("@TRANSACTION_TYPE", transactionType);
                //cmd.Parameters.AddWithValue("@INVOICE_TYPE", invoiceType);
                Encoding enc = Encoding.GetEncoding(855);
                cmd.Parameters.AddWithValue("@INVOICE_TYPE", enc.GetBytes(invoiceType));
                cmd.Parameters.AddWithValue("@TRANSACTION_TYPE", enc.GetBytes(transactionType));

                cmd.ExecuteNonQuery();
            }
        }
    }
}
