using LSCore.Contracts.Requests;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.ProductsGroups
{
    public class ProductsGroupUpdateTypeRequest : LSCoreSaveRequest
    {
        public ProductGroupType Type { get; set; }
    }
}