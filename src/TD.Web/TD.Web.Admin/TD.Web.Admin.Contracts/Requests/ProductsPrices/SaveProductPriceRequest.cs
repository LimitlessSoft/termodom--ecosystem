using TD.Core.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.ProductsPrices
{
    public class SaveProductPriceRequest : SaveRequest
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
    }
}
