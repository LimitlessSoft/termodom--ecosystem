using TD.Web.Admin.Contracts.Entities;

namespace TD.Web.Admin.Contracts.Dtos.Products
{
    public class ProductsGetGroupItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductsGetGroupItemDto? ParentGroup { get; set; }
    }
}
