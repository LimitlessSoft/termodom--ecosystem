using TD.Core.Contracts.Requests;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.Products
{
    public class ProductsSaveRequest : SaveRequest
    {
        public string Name { get; set; }
        public string? Src { get; set; }
        public string Image { get; set; }
        public string? CatalogId { get; set; }
        public int UnitId { get; set; }
        public ProductClassification Classification { get; set; }
        public decimal VAT { get; set; }
        public List<int> Groups { get; set; } = new List<int>();
        public int ProductPriceGroupId { get; set; }
    }
}
