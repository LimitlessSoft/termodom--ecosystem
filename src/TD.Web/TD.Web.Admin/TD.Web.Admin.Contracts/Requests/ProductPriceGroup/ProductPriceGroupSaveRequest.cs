using LSCore.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.ProductPriceGroup
{
    public class ProductPriceGroupSaveRequest : LSCoreSaveRequest
    {
        public string Name { get; set; }
    }
}
