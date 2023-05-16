namespace Termodom.Data.Entities.Komercijalno
{
    public class Magacin
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        public string MTID { get; set; }
        public int MozeMinus { get; set; }
        public int Vrsta { get; set; }
        public int? PFRID { get; set; }
    }
}
