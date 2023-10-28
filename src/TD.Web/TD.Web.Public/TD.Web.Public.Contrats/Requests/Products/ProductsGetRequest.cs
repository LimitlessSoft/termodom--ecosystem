namespace TD.Web.Public.Contrats.Requests.Products
{
    public class ProductsGetRequest
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? GroupId { get; set; }
        public string? KeywordSearch { get; set; }
    }
}
