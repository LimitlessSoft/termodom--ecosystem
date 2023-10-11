using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikKatBrRobaIds
{
    public class MCPartnerCenovnikKatBrRobaIdGetDto
    {
        public int Id { get; set; }
        public string KatBrProizvodjaca { get; set; }
        public int RobaId { get; set; }
        public int DobavljacPPID { get; set; }
    }
}
