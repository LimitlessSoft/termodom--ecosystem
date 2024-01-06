using LSCore.Contracts.Requests;
using TD.Web.Public.Contracts.Enums;

namespace TD.Web.Public.Contrats.Requests.Products
{
    public class ProductsGetRequest : LSCoreSortablePageableRequest<ProductsSortColumnCodes.Products>
    {
        public int? GroupId { get; set; }
        public string? KeywordSearch { get; set; }
    }
}
