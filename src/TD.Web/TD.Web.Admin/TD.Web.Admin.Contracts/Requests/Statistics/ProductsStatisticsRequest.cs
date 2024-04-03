namespace TD.Web.Admin.Contracts.Requests.Statistics
{
    public class ProductsStatisticsRequest
    {
        public DateTime DateFromUtc { get; set; }
        public DateTime DateToUtc { get; set; }
    }
}