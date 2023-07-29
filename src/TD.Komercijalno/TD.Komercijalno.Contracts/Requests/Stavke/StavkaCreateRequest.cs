using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Requests.Stavke
{
    public class StavkaCreateRequest
    {
        public int VrDok { get; set; }
        public int BrDok { get; set; }
        public int RobaId { get; set; }
        public string? Naziv { get; set; }
        public double? NabavnaCena { get; set; }
        public double? ProdajnaCenaBezPdv { get; set; }
        public double Kolicina { get; set; }
        public double Rabat { get; set; }
        public double DeviznaCena { get; set; } = 0;
        public double NivelisanaKolicina { get; set; } = 0;
        public double Marza { get; set; } = 0;
        public double ProsNab { get; set; } = 0;
        public double PreCena { get; set; } = 0;
        public double PreNab { get; set; } = 0;
        public double ProsProd { get; set; } = 0;
        public double DevProdCena { get; set; } = 0;
    }
}
