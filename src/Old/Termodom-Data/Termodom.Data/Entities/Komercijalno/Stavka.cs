
using System;

namespace Termodom.Data.Entities.Komercijalno
{
    public class Stavka
    {

        #region Properties
        public int StavkaID { get; set; }
        public int VrDok { get; set; }
        public int BrDok { get; set; }
        public int MagacinID { get; set; }
        public int RobaID { get; set; }
        public int? Vrsta { get; set; }
        public string Naziv { get; set; }
        public double? NabCenSaPor { get; set; }
        public double? FakturnaCena { get; set; }
        public double? NabCenaBT { get; set; }
        public double? Troskovi { get; set; }
        public double NabavnaCena { get; set; }
        public double ProdCenaBP { get; set; }
        public double? Korekcija { get; set; }
        public double ProdajnaCena { get; set; }
        public double DeviznaCena { get; set; }
        public double? DevProdCena { get; set; }
        public double Kolicina { get; set; }
        public double NivKol { get; set; }
        public string TarifaID { get; set; }
        public int? ImaPorez { get; set; }
        public double Porez { get; set; }
        public double Rabat { get; set; }
        public double Marza { get; set; }
        public double? Taksa { get; set; }
        public double? Akciza { get; set; }
        public double ProsNab { get; set; }
        public double PreCena { get; set; }
        public double PreNab { get; set; }
        public double ProsProd { get; set; }
        public string MTID { get; set; }
        public char PT { get; set; }
        public string Zvezdica { get; set; }
        public double? TrenStanje { get; set; }
        public double PorezUlaz { get; set; }
        public DateTime? SDatum { get; set; }
        public double? DevNabCena { get; set; }
        public double PorezIz { get; set; }
        public double? X4 { get; set; }
        public double? Y4 { get; set; }
        public double? Z4 { get; set; }
        public double? CenaPoAJM { get; set; }
        public int? KGID { get; set; }
        public double SAkciza { get; set; }
        #endregion

    }
}
