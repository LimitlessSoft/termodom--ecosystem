namespace TD.Web.Common.Contracts.Dtos.Users;

public class UsersAnalyzeOrderedProductsItemDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal QuantitySum { get; set; }
    public decimal ValueSum { get; set; }
    public decimal DiscountSum { get; set; }
}