namespace TD.Office.Public.Contracts.Dtos.NalogZaPrevoz
{
    public class GetReferentniDokumentNalogZaPrevozDto
    {
        public bool Zakljucan { get; set; }
        public DateTime Datum { get; set; }
        public decimal? VrednostStavkePrevozaBezPdv { get; set; }
        public bool PlacenVirmanom { get; set; }
    }
}