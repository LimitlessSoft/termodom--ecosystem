using System;

namespace Termodom.Data.Entities.Komercijalno
{
    public class Dokument
    {
        /// <summary>
        /// Vrsta dokumenta
        /// </summary>
        public int VrDok { get; set; }
        /// <summary>
        /// Broj dokumenta
        /// </summary>
        public int BrDok { get; set; }
        /// <summary>
        /// Interni Broj dokumenta
        /// </summary>
        public string IntBroj { get; set; }
        /// <summary>
        /// Flag dokumenta. 0 = Otkljucan, 1 = Zakljucan
        /// </summary>
        public int Flag { get; set; }
        /// <summary>
        /// Buffer polje fiskalizacije. 0 = nije fiskalizovan, 1 = fiskalizovan. Ne mora biti u pravu.
        /// </summary>
        public int Placen { get; set; }
        /// <summary>
        /// Datum dokumenta
        /// </summary>
        public DateTime Datum { get; set; }
        /// <summary>
        /// Datum roka dokumenta
        /// </summary>
        public DateTime? DatRoka { get; set; }
        /// <summary>
        /// Magacin ID dokumenta
        /// </summary>
        public int MagacinID { get; set; }
        /// <summary>
        /// Mesto troska ID dokumenta
        /// </summary>
        public string MTID { get; set; }
        /// <summary>
        /// Referent ID dokumenta
        /// </summary>
        public int RefID { get; set; }
        /// <summary>
        /// Zaposleni ID dokumenta
        /// </summary>
        public int ZapID { get; set; }
        /// <summary>
        /// KodDok dokumenta
        /// </summary>
        public int KodDok { get; set; }
        /// <summary>
        /// Duguje dokumenta
        /// </summary>
        public double Duguje { get; set; }
        /// <summary>
        /// Potrazuje dokumenta
        /// </summary>
        public double Potrazuje { get; set; }
        /// <summary>
        /// PPID dokumenta
        /// </summary>
        public int? PPID { get; set; }
        /// <summary>
        /// Nacin uplate ID. 1 = Virmanom, 5 = Gotovina
        /// </summary>
        public int NUID { get; set; }
        /// <summary>
        /// Troskovi dokumenta
        /// </summary>
        public double Troskovi { get; set; }
        /// <summary>
        /// Vrsta dokumenta iz koje je ovaj dokument prebacen
        /// </summary>
        public int? VrDokIn { get; set; }
        /// <summary>
        /// Broj dokumenta iz koje je ovaj dokument prebacen
        /// </summary>
        public int? BrDokIn { get; set; }
        /// <summary>
        /// Vrsta dokumenta u koji je ovaj dokument prebacen
        /// </summary>
        public int? VrDokOut { get; set; }
        /// <summary>
        /// Broj dokumenta u koji je ovaj dokument prebacen
        /// </summary>
        public int? BrDokOut { get; set; }
        /// <summary>
        /// Ne znam sta je ovo
        /// </summary>
        public int? MagID { get; set; }
        /// <summary>
        /// Alias U dokumenta
        /// </summary>
        public int? AliasU { get; set; }
        /// <summary>
        /// Opis uplate dokumenta
        /// </summary>
        public string OpisUpl { get; set; }
        /// <summary>
        /// Popust 1 Dana dokumenta
        /// </summary>
        public int? Popust1Dana { get; set; }
        /// <summary>
        /// Razlika dokumenta
        /// </summary>
        public double Razlika { get; set; }

    }
}
