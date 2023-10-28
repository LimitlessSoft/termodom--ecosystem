using TD.Core.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.ProductsGroups
{
    public class ProductsGroupsSaveRequest : SaveRequest
    {
        public string Name { get; set; }
        public int? ParentGroupId { get; set; }
    }
}
