namespace TD.Web.Admin.Contracts.Dtos.Statistics
{
    public class ProductsStatisticsViewsItemDto
    {
        public long ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Views { get; set; }
    }
}