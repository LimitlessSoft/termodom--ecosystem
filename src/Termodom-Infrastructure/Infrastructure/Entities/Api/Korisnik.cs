using System;
namespace Infrastructure.Entities.Api
{
    public class Korisnik
    {
        public int id { get; set; }
        public string ime { get; set; }
        public string pw { get; set; }
        public int tip { get; set; }
        public string nadimak { get; set; }
        public int status { get; set; }
        public string mobilni { get; set; }
        public string komentar { get; set; }
        public string mail { get; set; }
        public int poseta { get; set; }
        public string adresaStanovanja { get; set; }
        public string opstina { get; set; }
        public int magacinID { get; set; }
        public DateTime datumRodjenja { get; set; }
        public bool primaObavestenja { get; set; }
        public string pib { get; set; }
        public int? ppid { get; set; }
        public int? referent { get; set; }
        public DateTime datumKreiranja { get; set; }
        public bool? poslatRodjendanskiSMS { get; set; }
        public DateTime? datumOdobrenja { get; set; }
        public DateTime? poslednjiPutVidjen { get; set; }
        public int zanimanje { get; set; }
    }
}
