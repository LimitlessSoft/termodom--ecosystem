using TD.Core.Contracts.Requests;

namespace TD.Web.Veleprodaja.Contracts.Requests.Products
{
    public class ProductsPutRequest : SaveRequest
    {
        public string Name { get; set; }
        public string ThumbnailImagePath { get; set; }
        public string FullSizedImagePath { get; set; }
        public string SKU { get; set; }
        public string Unit { get; set; }
    }
}
