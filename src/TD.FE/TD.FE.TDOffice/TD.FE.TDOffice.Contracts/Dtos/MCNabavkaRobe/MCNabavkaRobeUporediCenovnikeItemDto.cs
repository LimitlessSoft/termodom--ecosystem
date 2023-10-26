namespace TD.FE.TDOffice.Contracts.Dtos.MCNabavkaRobe
{
    public class MCNabavkaRobeUporediCenovnikeItemDto
    {
        public int RobaId { get; set; }
        public string KatBr { get; set; }
        public string Naziv { get; set; }
        public string JM { get; set; }
        public List<MCNabavkaRobeUporediCenovnikeSubItemDto> SubItems { get; set; } = new List<MCNabavkaRobeUporediCenovnikeSubItemDto>();
    }
}
