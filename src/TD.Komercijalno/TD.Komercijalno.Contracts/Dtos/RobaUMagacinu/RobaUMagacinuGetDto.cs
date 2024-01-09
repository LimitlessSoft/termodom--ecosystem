using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Dtos.RobaUMagacinu
{
    public class RobaUMagacinuGetDto
    {
        public short MagacinId { get; set; }
        public int RobaId { get; set; }
        public int? PozicijaId { get; set; }
        public double NabavnaCena { get; set; }
        public double ProdajnaCena { get; set; }
        public double DeviznaNabavnaCena { get; set; }
        public double DeviznaProdajnaCena { get; set; }
        public double Stanje { get; set; }
        public double Naruceno { get; set; }
        public double Rezervisano { get; set; }
        public double StanjePoOtpremnici { get; set; }
        public double EvidentiranoStanje { get; set; }
        public double OptimalnaZaliha { get; set; }
        public double? StanjePoSerijama { get; set; }
        public double KriticnaZaliha { get; set; }
        public double MesecniProsekProdaje { get; set; }
        public double StanjePoReklamacijama { get; set; }
        public double StanjePoReversu { get; set; }
        public double WmsStanje { get; set; }
    }
}
