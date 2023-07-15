namespace TD.FE.TDOffice.Contracts.Dtos.TabelarniPregledIzvoda
{
    public class TabelarniPregledIzvodaGetDto
    {
        public string FirmaPib { get; set; } = "";
        public int BrDok { get; set; }
        public int VrDok { get; set; }
        public string? IntBroj { get; set; }
        public DateTime DatumIzvoda { get; set; }
        public decimal UnosPocetnoStanje { get; set; }
        public decimal UnosPotrazuje { get; set; }
        public decimal UnosDuguje { get; set; }
        public decimal NovoStanje { get => UnosPotrazuje - UnosDuguje; }
        public int Korisnik { get; set; }
        public bool FinansijskaIspravnost { get; set; }
        public bool Zakljucano { get; set; }
        public bool LogickaIspravnost { get; set; }
    }
}
