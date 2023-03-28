using FirebirdSql.Data.FirebirdClient;
using System.Text;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDBrain_v3.Managers.TDOffice_v2
{
    public static class FiskalniRacunTaxItemManager
    {
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

            Dictionary<string, FiskalniRacunTaxItem> dict = new Dictionary<string, FiskalniRacunTaxItem>();
            using (FbCommand cmd = new FbCommand("SELECT * FROM FISKALNI_RACUN_TAX_ITEM " + whereQuery, con))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding enc = Encoding.GetEncoding(855);
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                    {
                        dict.Add(dr["INVOICE_NUMBER"].ToString(), new FiskalniRacunTaxItem()
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
