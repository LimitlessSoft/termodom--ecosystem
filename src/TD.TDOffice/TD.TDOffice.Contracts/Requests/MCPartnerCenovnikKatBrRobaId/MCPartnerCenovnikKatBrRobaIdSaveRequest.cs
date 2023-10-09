using TD.Core.Contracts.Requests;

namespace TD.TDOffice.Contracts.Requests.MCPartnerCenovnikKatBrRobaId
{
    public class MCPartnerCenovnikKatBrRobaIdSaveRequest : SaveRequest
    {
        public string KatBrProizvodjaca { get; set; }
        public int RobaId { get; set; }
        public string Proizvodjac { get; set; }
    }
}
