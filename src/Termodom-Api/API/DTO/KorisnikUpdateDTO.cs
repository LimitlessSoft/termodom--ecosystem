using API.Models;
using System;

namespace API.DTO
{
    /// <summary>
    /// DTO Objekat
    /// </summary>
    public class KorisnikUpdateDTO
    {
        public Int16 ID { get; set; }
        public string Ime { get; set; }
        public string PW { get; set; }
        public KorisnikTip Tip { get; set; } // Remove
        public int Status { get; set; }
        public string Mobilni { get; set; }
        public string Mail { get; set; }
        public string AdresaStanovanja { get; set; }
        public string Opstina { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public Int16? PPID { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public bool PoslatRodjendanskiSMS { get; set; }
        public string PIB { get; set; }
    }
}
