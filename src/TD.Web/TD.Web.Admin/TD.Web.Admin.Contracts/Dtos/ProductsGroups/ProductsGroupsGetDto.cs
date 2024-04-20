namespace TD.Web.Admin.Contracts.Dtos.ProductsGroups
{
    public class ProductsGroupsGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentGroupId { get; set; }
        public string? WelcomeMessage { get; set; }
        public int TypeId { get; set; }
    }
}
