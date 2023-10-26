using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class KorisnikInsertDTO
    {
        public string Ime { get; set; }
        public string PW { get; set; }
        public Models.KorisnikTip Tip { get; set; }
        public string Nadimak { get; set; }
        public string Mobilni { get; set; }
        public string Komentar { get; set; }
        public string Mail { get; set; }
        public string AdresaStanovanja { get; set; }
        public string Opstina { get; set; }
        public Int16 MagacinID { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string PIB { get; set; }
        public int Zanimanje { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
