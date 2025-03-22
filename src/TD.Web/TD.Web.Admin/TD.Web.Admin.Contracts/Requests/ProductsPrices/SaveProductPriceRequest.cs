namespace TD.Web.Admin.Contracts.Requests.ProductsPrices;

public class SaveProductPriceRequest
{
	public long? Id { get; set; }
	public decimal Min { get; set; }
	public decimal Max { get; set; }
}
