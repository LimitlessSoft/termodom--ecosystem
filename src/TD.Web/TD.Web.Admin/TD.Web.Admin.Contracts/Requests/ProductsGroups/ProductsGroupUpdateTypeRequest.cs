using TD.Web.Common.Contracts.Enums;
using LSCore.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.ProductsGroups;

public class ProductsGroupUpdateTypeRequest : LSCoreSaveRequest
{
    public ProductGroupType Type { get; set; }
}