using TD.Web.Contracts.Entities;

namespace TD.Web.Contracts.Dtos.ProductsGroups
{
    public class ProductsGroupsGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentGroupId { get; set; }
    }
}
