namespace Termodom.Data.Entities.TDOffice_v2
{
    public class Firma
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        public string TR { get; set; }
        public string PIB { get; set; }
        public string MB { get; set; }
        public string Adresa { get; set; }
        public string Grad { get; set; }
        public int PPID { get; set; }
        public int GlavniMagacin { get; set; }
        public int MagacinRazduzenja { get; set; }
    }
}
