using LSCore.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.ProductsPrices
{
    public class SaveProductPriceRequest : LSCoreSaveRequest
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
    }
}
