using LSCore.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.ProductsGroups
{
    public class ProductsGroupsSaveRequest : LSCoreSaveRequest
    {
        public string Name { get; set; }
        public int? ParentGroupId { get; set; }
    }
}
