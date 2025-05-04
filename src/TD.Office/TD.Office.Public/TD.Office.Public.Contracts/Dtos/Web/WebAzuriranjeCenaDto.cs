using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Dtos.Web;

public class WebAzuriranjeCenaDto
{
	public long Id { get; set; }
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
	public long? LinkId { get; set; }
	public long UslovFormiranjaWebCeneId { get; set; }
	public UslovFormiranjaWebCeneType UslovFormiranjaWebCeneType { get; set; }
	public decimal UslovFormiranjaWebCeneModifikator { get; set; }
}
