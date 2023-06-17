namespace TD.WebshopListener.Contracts.Responses
{
    public class GetRobaResponse
    {
        public int ID { get; set; }

        public string KatBr { get; set; }

        public string KatBrPro { get; set; }

        public string Naziv { get; set; }

        public string JM { get; set; }

        public string TarifaID { get; set; }

        public int Vrsta { get; set; }

        public string NazivZaStampu { get; set; }

        public string GrupaID { get; set; }

        public int? Podgrupa { get; set; }

        public string ProID { get; set; }

        public int? DOB_PPID { get; set; }

        public string TrPak { get; set; }

        public double? TrKol { get; set; }
    }
}
