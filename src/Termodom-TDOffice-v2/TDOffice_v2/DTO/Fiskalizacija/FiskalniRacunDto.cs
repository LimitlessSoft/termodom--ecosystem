using System.Collections.Generic;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDOffice_v2.DTO.Fiskalizacija
{
    public partial class FiskalniRacunDto : FiskalniRacun
    {
        public List<FiskalniRacunTaxItem> TaxItems { get; set; }
        public List<FiskalniRacunPaymentItem> Payments { get; set; }
    }
}
