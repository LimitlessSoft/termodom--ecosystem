namespace TD.TDOffice.Contracts.Requests.DokumentTagIzvod
{
    public class DokumentTagIzvodGetMultipleRequest
    {
        public int? BrDok { get; set; }
        public List<int>? Korisnici { get; set; }
    }
}
