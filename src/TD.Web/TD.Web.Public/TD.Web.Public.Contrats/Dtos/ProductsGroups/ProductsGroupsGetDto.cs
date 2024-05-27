using LSCore.Contracts.Dtos;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Public.Contracts.Dtos.ProductsGroups
{
    public class ProductsGroupsGetDto : LSCoreIdNamePairDto
    {
        public string? ParentName { get; set; }
        public string? WelcomeMessage { get; set; }
        public ProductGroupType Type { get; set; } 
    }
}
