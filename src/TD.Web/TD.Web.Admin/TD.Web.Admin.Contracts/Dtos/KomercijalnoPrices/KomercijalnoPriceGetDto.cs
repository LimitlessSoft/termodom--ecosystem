namespace TD.Web.Admin.Contracts.Dtos.KomercijalnoPrices
{
    public class KomercijalnoPriceGetDto
    {
        public int RobaId { get; set; }
        public decimal NabavnaCenaBezPDV { get; set; }
        public decimal ProdajnaCenaBezPDV { get; set; }
    }
}
