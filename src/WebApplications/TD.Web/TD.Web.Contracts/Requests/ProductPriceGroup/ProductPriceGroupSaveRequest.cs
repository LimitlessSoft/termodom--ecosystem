using TD.Core.Contracts.Requests;

namespace TD.Web.Contracts.Requests.ProductPriceGroup
{
    public class ProductPriceGroupSaveRequest : SaveRequest
    {
        public string Name { get; set; }
    }
}
