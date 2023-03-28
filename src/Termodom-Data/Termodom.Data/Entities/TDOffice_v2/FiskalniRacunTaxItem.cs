namespace Termodom.Data.Entities.TDOffice_v2
{
    /// <summary>
    /// 
    /// </summary>
    public class FiskalniRacunTaxItem
    {
        public int ID { get; set; }
        public string InvoiceNumber { get; set; }
        public string Label { get; set; }
        public double Amount { get; set; }
        public double Rate { get; set; }
        public string CategoryName { get; set; }
    }
}
