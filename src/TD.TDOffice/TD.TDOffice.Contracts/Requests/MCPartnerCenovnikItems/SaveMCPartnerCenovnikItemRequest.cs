using LSCore.Contracts.Requests;

namespace TD.TDOffice.Contracts.Requests.MCPartnerCenovnikItems
{
    public class SaveMCPartnerCenovnikItemRequest : LSCoreSaveRequest
    {
        public string KatBr { get; set; }
        public double VpCenaBezRabata { get; set; }
        public double Rabat { get; set; }
        public int PPID { get; set; }
        public DateTime VaziOdDana { get; set; }
    }
}
