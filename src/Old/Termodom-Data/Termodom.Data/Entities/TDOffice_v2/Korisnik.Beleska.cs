
namespace Termodom.Data.Entities.TDOffice_v2
{
    public partial class Korisnik
    {
        public class Beleska
        {
            public int Id { get; set; }
            public int KorisnikId { get; set; }
            public string Naslov { get; set; }
            public string Body { get; set; }
        }
    }
}
