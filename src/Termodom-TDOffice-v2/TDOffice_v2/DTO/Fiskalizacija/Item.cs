using System.Collections.Generic;

namespace TDOffice_v2.DTO.Fiskalizacija
{
    public partial class FiskalniRacun
    {
        public class Item
        {
            public string GTIN { get; set; }
            public string Name { get; set; }
            public double Quantity { get; set; }
            public List<string> Labels { get; set; }
            public double UnitPrice { get; set; }
            public double TotalAmount { get; set; }
        }
    }
}
