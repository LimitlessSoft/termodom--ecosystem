using TD.Web.Contracts.Entities;

namespace TD.Web.Contracts.Dtos.Products
{
    public class ProductsGetMultipleGroupItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductsGetMultipleGroupItemDto? ParentGroup { get; set; }
    }
}
