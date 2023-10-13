using System.ComponentModel.DataAnnotations.Schema;

namespace TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikItems
{
    public class MCpartnerCenovnikItemEntityGetDto
    {
        public int Id { get; set; }
        public string KatBr { get; set; }
        public double VpCenaBezRabata { get; set; }
        public double Rabat { get; set; }
        public int PPID { get; set; }
        public DateTime VaziOdDana { get; set; }
    }
}
