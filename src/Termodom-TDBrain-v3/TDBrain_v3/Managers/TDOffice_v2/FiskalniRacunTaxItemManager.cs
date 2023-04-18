using FirebirdSql.Data.FirebirdClient;
using System.Reflection.Emit;
using System.Text;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDBrain_v3.Managers.TDOffice_v2
{
    public static class FiskalniRacunTaxItemManager
    {
        public static void Insert(FiskalniRacunTaxItem taxItem)
        {
            using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringTDOffice_v2.ConnectionString()))
            {
                con.Open();
                Insert(con, taxItem);
            }
        }
        public static void Insert(FbConnection con, FiskalniRacunTaxItem taxItem)
        {
            using (FbCommand cmd = new FbCommand("INSERT INTO FISKALNI_RACUN_TAX_ITEM (ID, INVOICE_NUMBER, LABEL, AMOUNT, RATE, CATEGORY_NAME) VALUES (((SELECT COALESCE(MAX(ID), 0) FROM FISKALNI_RACUN_TAX_ITEM) + 1), @IN, @L, @A, @R, @C)", con))
            {
                cmd.Parameters.AddWithValue("@IN", taxItem.InvoiceNumber);
                cmd.Parameters.AddWithValue("@L", taxItem.Label);
                cmd.Parameters.AddWithValue("@A", taxItem.Amount);
                cmd.Parameters.AddWithValue("@R", taxItem.Rate);
                cmd.Parameters.AddWithValue("@C", taxItem.CategoryName);

                cmd.ExecuteNonQuery();
            }
        }
        public static FiskalniRacunTaxItemDictionary Dictionary(List<string> whereParameters = null)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringTDOffice_v2.ConnectionString()))
            {
                con.Open();
                return Dictionary(con, whereParameters);
            }
        }
        public static FiskalniRacunTaxItemDictionary Dictionary(FbConnection con, List<string> whereParameters = null)
        {
            string whereQuery = "";

            if (whereParameters != null && whereParameters.Count > 0)
                whereQuery = " WHERE " + string.Join(" AND ", whereParameters);

            Dictionary<string, List<FiskalniRacunTaxItem>> dict = new Dictionary<string, List<FiskalniRacunTaxItem>>();
            using (FbCommand cmd = new FbCommand("SELECT * FROM FISKALNI_RACUN_TAX_ITEM " + whereQuery, con))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding enc = Encoding.GetEncoding(855);
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                    {
                        if (!dict.ContainsKey(dr["INVOICE_NUMBER"].ToString()))
                            dict.Add(dr["INVOICE_NUMBER"].ToString(), new List<FiskalniRacunTaxItem>());

                        dict[dr["INVOICE_NUMBER"].ToString()].Add(new FiskalniRacunTaxItem()
                        {
                            InvoiceNumber = dr["INVOICE_NUMBER"].ToString(),
                            ID = Convert.ToInt32(dr["ID"]),
                            Label = dr["LABEL"].ToString(),
                            Amount = Convert.ToDouble(dr["AMOUNT"]),
                            Rate = Convert.ToDouble(dr["RATE"]),
                            CategoryName = dr["CATEGORY_NAME"].ToString()
                        });
                    }
            }
            return new FiskalniRacunTaxItemDictionary(dict);
        }
    }
}
