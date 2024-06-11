using TD.Web.Public.Contracts.Enums;
using LSCore.Contracts.Requests;

namespace TD.Web.Public.Contracts.Requests.Products;

public class ProductsGetRequest : LSCoreSortablePageableRequest<ProductsSortColumnCodes.Products>
{
    public string? GroupName { get; set; }
    public string? KeywordSearch { get; set; }
    public List<long>? Ids { get; set; }
}