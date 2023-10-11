using Microsoft.AspNetCore.Http;

namespace TD.FE.TDOffice.Contracts.Requests.MCNabavkaRobe
{
    public class MCNabavkaRobeUvuciFajlRequest
    {
        public IFormFile File { get; set; }
        public int KolonaKataloskiBroj { get; set; }
        public int KolonaNaziv { get; set; }
        public int KolonaJediniceMere { get; set; }
        public int DobavljacPPID { get; set; }
        //public Dictionary<string, string> JMEqueals { get; set; } = new Dictionary<string, string>();
    }
}
