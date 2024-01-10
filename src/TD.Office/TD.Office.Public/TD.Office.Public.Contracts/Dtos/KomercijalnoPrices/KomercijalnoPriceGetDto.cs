namespace TD.Office.Public.Contracts.Dtos.KomercijalnoPrices
{
    public class KomercijalnoPriceGetDto
    {
        public int RobaId { get; set; }
        public decimal NabavnaCenaBezPDV { get; set; }
        public decimal ProdajnaCenaBezPDV { get; set; }
    }
}
