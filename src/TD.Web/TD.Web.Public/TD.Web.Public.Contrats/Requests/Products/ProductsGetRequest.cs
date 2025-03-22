using LSCore.SortAndPage.Contracts;
using TD.Web.Public.Contracts.Enums;

namespace TD.Web.Public.Contracts.Requests.Products;

public class ProductsGetRequest : LSCoreSortableAndPageableRequest<ProductsSortColumnCodes.Products>
{
	public string? GroupName { get; set; }
	public string? KeywordSearch { get; set; }
	public List<long>? Ids { get; set; }
}
