namespace TDOffice_v2.DTO.Fiskalizacija
{
    public partial class FiskalniRacun
    {
        public class TaxItem
        {
            public string Label { get; set; }
            public double Amount { get; set; }
            public double Rate { get; set; }
            public string CategoryName { get; set; }
        }
    }
}
