using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public class FiskalniRacun_TaxItem
    {
        public int ID { get; set; }
        public string InvoiceNumber { get; set; }
        public string Label { get; set; }
        public double Amount { get; set; }
        public double Rate { get; set; }
        public string CategoryName { get; set; }

        public static FiskalniRacun_TaxItem Get(int id)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Get(con, id);
            }
        }
        public static FiskalniRacun_TaxItem Get(FbConnection con, int id)
        {
            using(FbCommand cmd = new FbCommand("SELECT ID, INVOICE_NUMBER, LABEL, AMOUNT, RATE, CATEGORY_NAME FROM FISKALNI_RACUN_TAX_ITEM WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new FiskalniRacun_TaxItem()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            InvoiceNumber = dr["INVOICE_NUMBER"].ToString(),
                            Label = dr["LABEL"].ToString(),
                            Amount = Convert.ToDouble(dr["AMOUNT"]),
                            Rate = Convert.ToDouble(dr["RATE"]),
                            CategoryName = dr["CATEGORY_NAME"].ToString()
                        };
            }
            return null;
        }
        public static List<FiskalniRacun_TaxItem> List(string whereQuery = null)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return List(con, whereQuery);
            }
        }
        public static List<FiskalniRacun_TaxItem> List(FbConnection con, string whereQuery = null)
        {
            if (!string.IsNullOrWhiteSpace(whereQuery))
                whereQuery = " WHERE " + whereQuery;

            List<FiskalniRacun_TaxItem> list = new List<FiskalniRacun_TaxItem>();

            using (FbCommand cmd = new FbCommand("SELECT ID, INVOICE_NUMBER, LABEL, AMOUNT, RATE, CATEGORY_NAME FROM FISKALNI_RACUN_TAX_ITEM " + whereQuery, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new FiskalniRacun_TaxItem()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            InvoiceNumber = dr["INVOICE_NUMBER"].ToString(),
                            Label = dr["LABEL"].ToString(),
                            Amount = Convert.ToDouble(dr["AMOUNT"]),
                            Rate = Convert.ToDouble(dr["RATE"]),
                            CategoryName = dr["CATEGORY_NAME"].ToString()
                        });
            }
            return list;
        }
        public static void Insert(string invoiceNumber, string label, double amount, double rate, string categoryName)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Insert(con, invoiceNumber, label, amount, rate, categoryName);
            }
        }
        public static void Insert(FbConnection con, string invoiceNumber, string label, double amount, double rate, string categoryName)
        {
            using(FbCommand cmd = new FbCommand("INSERT INTO FISKALNI_RACUN_TAX_ITEM (ID, INVOICE_NUMBER, LABEL, AMOUNT, RATE, CATEGORY_NAME) VALUES (((SELECT COALESCE(MAX(ID), 0) FROM FISKALNI_RACUN_TAX_ITEM) + 1), @IN, @L, @A, @R, @C)", con))
            {
                cmd.Parameters.AddWithValue("@IN", invoiceNumber);
                cmd.Parameters.AddWithValue("@L", label);
                cmd.Parameters.AddWithValue("@A", amount);
                cmd.Parameters.AddWithValue("@R", rate);
                cmd.Parameters.AddWithValue("@C", categoryName);

                cmd.ExecuteNonQuery();
            }
        }
        public static void Delete(int id)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Delete(con, id);
            }
        }
        public static void Delete(FbConnection con, int id)
        {
            using(FbCommand cmd = new FbCommand("DELETE FROM FISKALNI_RACUN_TAX_ITEM WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
