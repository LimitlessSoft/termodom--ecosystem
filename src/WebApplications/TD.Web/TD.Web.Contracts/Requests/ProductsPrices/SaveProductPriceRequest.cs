using TD.Core.Contracts.Requests;

namespace TD.Web.Contracts.Requests.ProductsPrices
{
    public class SaveProductPriceRequest : SaveRequest
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
    }
}
