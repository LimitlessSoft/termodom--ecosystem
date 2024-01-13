namespace TD.Office.Public.Contracts.Dtos.Web
{
    public class WebAzuriranjeCenaDto
    {
        public string Naziv { get; set; } = "Undefined";
        public decimal MinWebOsnova { get; set; }
        public decimal MaxWebOsnova { get; set; }
        public decimal NabavnaCenaKomercijalno { get; set; }
        public decimal ProdajnaCenaKomercijalno { get; set; }
        public decimal IronCena { get; set; }
        public decimal SilverCena { get; set; }
        public decimal GoldCena { get; set; }
        public decimal PlatinumCena { get; set; }
        public int? LinkRobaId { get; set; }
        public int? LinkId { get; set; }
    }
}
