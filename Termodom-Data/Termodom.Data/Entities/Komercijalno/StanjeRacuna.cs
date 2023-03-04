using System;

namespace Termodom.Data.Entities.Komercijalno
{
    public class StanjeRacuna
    {
        public string Racun { get; set; }
        public int PPID { get; set; }
        public double PocDuguje { get; set; }
        public double PocPotrazuje { get; set; }
        public double Duguje { get; set; }
        public double Potrazuje { get; set; }
        public int? KasaID { get; set; }
        public double? DozvoljeniMinus { get; set; }
        public DateTime? DozvoljeniMinusVaziDo { get; set; }
    }
}
