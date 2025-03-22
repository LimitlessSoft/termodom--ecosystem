using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Public.Contracts.Interfaces.IManagers;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
[Permissions(Permission.Access)]
public class EnumsController(IEnumManager enumManager) : ControllerBase
{
	[HttpGet]
	[Route("/product-stock-types")]
	public List<IdNamePairDto> GetProductStockTypes() => enumManager.GetProductStockTypes();
}
