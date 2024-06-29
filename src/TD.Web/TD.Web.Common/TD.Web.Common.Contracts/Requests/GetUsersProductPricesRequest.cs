namespace TD.Web.Common.Contracts.Requests;

public class GetUsersProductPricesRequest
{
    public long UserId { get; set; }
    public long ProductId { get; set; }
}