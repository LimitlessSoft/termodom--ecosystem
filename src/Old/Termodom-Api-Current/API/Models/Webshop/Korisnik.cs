using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Webshop
{
    /// <summary>
    /// WebShop korisnik
    /// </summary>
    public class Korisnik : Models.Korisnik
    {
        public Int16 Zanimanje { get; set; }
        public int Poseta { get; set; }
        public Int16 MagacinID { get; set; }
        public bool PrimaObavestenja { get; set; }
        public Int16? Referent { get; set; }
        public DateTime? DatumOdobrenja { get; set; }
        public DateTime? PoslednjiPutVidjen { get; set; }
        public string Komentar { get; set; }
        public string Nadimak { get; set; }

    }
}
