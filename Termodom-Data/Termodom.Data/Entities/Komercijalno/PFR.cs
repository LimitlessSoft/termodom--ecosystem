namespace Termodom.Data.Entities.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    public class PFR
    {
        public int PFRID { get; set; }
        public string Name { get; set; }
        public string Verzija { get; set; }
        public string Url { get; set; }
        public int CanPrint { get; set; }
        public string PrinterName { get; set; }
        public int? ReportID { get; set; }
        public string JID { get; set; }
        public int AddRefNbr { get; set; }
    }
}
