using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Public.Contracts.Dtos.ProductsGroups;

public class ProductsGroupsGetDto : IdNamePairDto
{
	public string? ParentName { get; set; }
	public string ParentSrc { get; set; }
	public string? WelcomeMessage { get; set; }
	public string? SalesMobile { get; set; }
	public ProductGroupType Type { get; set; }
	public string Src { get; set; }
}
