using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO.Webshop
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class PorudzbinaInsertDTO
    {
        public int KorisnikID { get; set; }
        public int? BrDokKomercijalno { get; set; }
        public DateTime Datum { get; set; }
        public int Status { get; set; }
        public int MagacinID { get; set; }
        public int? PPID { get; set; }
        public string InterniKomentar { get; set; }
        public int? ReferentObrade { get; set; }
        public int NacinPlacanja { get; set; }
        public string Hash { get; set; }
        public double K { get; set; }
        public double UstedaKorisnik { get; set; }
        public double UstedaKlijent { get; set; }
        public int Dostava { get; set; }
        public string KomercijalnoInterniKomentar { get; set; }
        public string KomercijalnoKomentar { get; set; }
        public string KomercijalnoNapomena { get; set; }
        public string Napomena { get; set; }
        public string AdresaIsporuke { get; set; }
        public string KontaktMobilni { get; set; }
        public string ImeIPrezime { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
