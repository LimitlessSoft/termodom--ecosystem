using System;
using Termodom.Data.Enumerators;

namespace Termodom.Data.Entities.TDOffice_v2
{
    public class Popis
    {
        public int ID { get; set; }
        public DateTime Datum { get; set; }
        public int MagacinID { get; set; }
        public int UserID { get; set; }
        public PopisStatus Status { get; set; }
        public string Komentar { get; set; }
        public string InterniKomentar { get; set; }
        public PopisTip Tip { get; set; }
        public int? KomercijalnoPopisBrDok { get; set; }
        public int? KomercijalnoNarudzbenicaBrDok { get; set; }
        public int SpecijalStampa { get; set; }
        public int? ZaduzenjeBrDokKomercijalno { get; set; }
        public string Napomena { get; set; }
    }
}
