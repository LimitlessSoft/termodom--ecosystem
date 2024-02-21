using LSCore.Contracts.Dtos;

namespace TD.Web.Public.Contracts.Dtos.ProductsGroups
{
    public class ProductsGroupsGetDto : LSCoreIdNamePairDto
    {
        public string? ParentName { get; set; }
    }
}
