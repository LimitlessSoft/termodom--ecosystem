using TD.Core.Contracts.Requests;

namespace TD.TDOffice.Contracts.Requests.DokumentTagIzvod
{
    public class DokumentTagizvodPutRequest : SaveRequest
    {
        public int? BrojDokumentaIzvoda { get; set; }
        public decimal? UnosPocetnoStanje { get; set; }
        public decimal? UnosPotrazuje { get; set; }
        public decimal? UnosDuguje { get; set; }
        public int? Korisnik { get; set; }
    }
}
