using TD.Web.Contracts.Entities;

namespace TD.Web.Contracts.Dtos.Products
{
    public class ProductsGetGroupItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductsGetGroupItemDto? ParentGroup { get; set; }
    }
}
