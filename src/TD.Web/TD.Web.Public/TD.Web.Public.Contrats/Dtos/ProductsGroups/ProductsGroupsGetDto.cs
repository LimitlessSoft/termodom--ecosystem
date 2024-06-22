using TD.Web.Common.Contracts.Enums;
using LSCore.Contracts.Dtos;

namespace TD.Web.Public.Contracts.Dtos.ProductsGroups;

public class ProductsGroupsGetDto : LSCoreIdNamePairDto
{
    public string? ParentName { get; set; }
    public string? WelcomeMessage { get; set; }
    public ProductGroupType Type { get; set; } 
}