using LSCore.Auth.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers;

[LSCoreAuth]
[ApiController]
[Permissions(Permission.Access)]
public class EnumsController(IEnumManager enumManager) : ControllerBase
{
	[HttpGet]
	[Route("/order-statuses")]
	public List<IdNamePairDto> GetOrderStatuses() => enumManager.GetOrderStatuses();

	[HttpGet]
	[Route("/user-types")]
	public List<IdNamePairDto> GetUserTypes() => enumManager.GetUserTypes();

	[HttpGet]
	[Route("/product-group-types")]
	public List<IdNamePairDto> GetProductGroupTypes() => enumManager.GetProductGroupTypes();

	[HttpGet]
	[Route("/product-stock-types")]
	public List<IdNamePairDto> GetProductStockTypes() => enumManager.GetProductStockTypes();

	[HttpGet]
	[Route("/calculator-types")]
	public List<IdNamePairDto> GetCalculatorTypes() => enumManager.GetCalculatorTypes();
}
