namespace TD.WebshopListener.Contracts.Dtos
{
    public class StavkaDto
    {
        public int ID { get; set; }
        public int PorudzbinaID { get; set; }
        public int RobaID { get; set; }
        public double Kolicina { get; set; }
        public double VpCena { get; set; }
        public double Rabat { get; set; }
    }
}
