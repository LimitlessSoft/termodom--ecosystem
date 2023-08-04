using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.Magacini;

namespace TD.Komercijalno.Contracts.Dtos.Stavke
{
    public class StavkaDto
    {
        public int Id { get; set; }
        public int RobaId { get; set; }
        public short? Vrsta { get; set; }
        public string? Naziv { get; set; }
        public double NabCenSaPor { get; set; }
        public double FakturnaCena { get; set; }
        public double NabavnaCena { get; set; }
        public double ProdCenaBp { get; set; }
        public double Korekcija { get; set; }
        public double ProdajnaCena { get; set; }
        public double Kolicina { get; set; }
        public string TarifaId { get; set; }
        public short? ImaPorez { get; set; }
        public double Porez { get; set; }
        public double PorezUlaz { get; set; }
        public double PorezIzlaz { get; set; }
        public string MtId { get; set; }
        public double DeviznaCena { get; set; }
        public double NivelisanaKolicina { get; set; }
        public double Rabat { get; set; }
        public double Marza { get; set; }
        public double ProsNab { get; set; }
        public double PreCena { get; set; }
        public double PreNab { get; set; }
        public double ProsProd { get; set; }
        //public DokumentDto Dokument { get; set; }
        //public MagacinDto Magacin { get; set; }
    }
}
