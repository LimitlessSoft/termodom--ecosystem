using LSCore.Contracts.Requests;

namespace TD.TDOffice.Contracts.Requests.MCPartnerCenovnikKatBrRobaId
{
    public class MCPartnerCenovnikKatBrRobaIdSaveRequest : LSCoreSaveRequest
    {
        public string KatBrProizvodjaca { get; set; }
        public int RobaId { get; set; }
        public int DobavljacPPID { get; set; }
    }
}
