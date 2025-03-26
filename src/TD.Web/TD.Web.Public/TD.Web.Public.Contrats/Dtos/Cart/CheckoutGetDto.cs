namespace TD.Web.Public.Contracts.Dtos.Cart;

public class CheckoutGetDto
{
    public string? OneTimeHash { get; set; }
    public long FavoriteStoreId { get; set; }
    public long PaymentTypeId { get; set; }
}