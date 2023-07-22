namespace TD.TDOffice.Contracts.Requests.DokumentTagIzvod
{
    public class DokumentTagizvodPutRequest
    {
        public int? Id { get; set; } // null
        public int? BrojDokumentaIzvoda { get; set; } // 900
        public decimal UnosPocetnoStanje { get; set; }
        public decimal UnosPotrazuje { get; set; }
        public decimal UnosDuguje { get; set; }
        public int Korisnik { get; set; }
    }
}
