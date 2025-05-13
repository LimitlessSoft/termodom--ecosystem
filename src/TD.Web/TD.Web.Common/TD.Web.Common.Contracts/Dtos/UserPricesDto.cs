namespace TD.Web.Common.Contracts.Dtos;

public class UserPricesDto
{
	public decimal PriceWithoutVAT { get; set; }
	public decimal VAT { get; set; }
	public decimal PriceWithVAT
	{
		get => PriceWithoutVAT * ((100 + VAT) / 100);
	}
}
