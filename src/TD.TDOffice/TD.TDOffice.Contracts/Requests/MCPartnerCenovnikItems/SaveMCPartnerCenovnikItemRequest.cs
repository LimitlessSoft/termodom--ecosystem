using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.TDOffice.Contracts.Entities;

namespace TD.TDOffice.Contracts.Requests.MCPartnerCenovnikItems
{
    public class SaveMCPartnerCenovnikItemRequest : SaveRequest
    {
        public string KatBr { get; set; }
        public double VpCenaBezRabata { get; set; }
        public double Rabat { get; set; }
        public int PPID { get; set; }
        public DateTime VaziOdDatana { get; set; }
    }
}
