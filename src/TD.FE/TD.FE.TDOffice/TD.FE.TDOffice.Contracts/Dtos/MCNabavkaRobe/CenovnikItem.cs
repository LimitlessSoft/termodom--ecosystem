namespace TD.FE.TDOffice.Contracts.Dtos.MCNabavkaRobe
{
    public class CenovnikItem
    {
        public string? KatBr { get; set; }
        public string KatBrPro { get; set; }
        public string? Naziv { get; set; }
        public string NazivPro { get; set; }
        public string? JM { get; set; }
        public string JMPro { get; set; }
        public double VPCenaSaRabatom { get; set; }
        public bool FoundInRoba { get; set; }
        public int? VezaId { get; set; }
        public int? KomercijalnoRobaId { get; set; }
    }
}
