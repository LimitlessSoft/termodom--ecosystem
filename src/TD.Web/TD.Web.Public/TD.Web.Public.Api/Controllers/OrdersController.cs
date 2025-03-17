using LSCore.Auth.Contracts;
using LSCore.SortAndPage.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Public.Contracts.Dtos.Orders;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Orders;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
public class OrdersController(IOrderManager orderManager) : ControllerBase
{
	[HttpGet]
	[LSCoreAuth]
	[Route("/orders")]
	public LSCoreSortedAndPagedResponse<OrdersGetDto> GetMultiple(
		[FromQuery] GetMultipleOrdersRequest request
	) => orderManager.GetMultiple(request);

	[HttpGet]
	[LSCoreAuth]
	[Route("/orders-info")]
	public OrdersInfoDto GetOrdersInfo() => orderManager.GetOrdersInfo();

	[HttpGet]
	[Route("/orders/{OneTimeHash}")]
	public OrderGetSingleDto GetSingle([FromRoute] GetSingleOrderRequest request) =>
		orderManager.GetSingle(request);
}
