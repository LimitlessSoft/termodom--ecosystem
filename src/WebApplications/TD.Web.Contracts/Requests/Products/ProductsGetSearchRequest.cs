namespace TD.Web.Contracts.Requests.Products
{
    public class ProductsGetSearchRequest : ProductsGetMultipleRequest
    {
        public string? SearchTerm { get; set; }
    }
}
