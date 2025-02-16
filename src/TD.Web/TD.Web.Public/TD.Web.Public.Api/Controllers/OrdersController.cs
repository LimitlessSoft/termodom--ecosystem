using LSCore.Contracts.Responses;
using LSCore.Framework.Attributes;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Orders;
using TD.Web.Public.Contracts.Dtos.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
public class OrdersController (IOrderManager orderManager) : ControllerBase
{
    [HttpGet]
    [LSCoreAuthorize]
    [Route("/orders")]
    public LSCoreSortedAndPagedResponse<OrdersGetDto> GetMultiple([FromQuery]GetMultipleOrdersRequest request) =>
        orderManager.GetMultiple(request);

    [HttpGet]
    [LSCoreAuthorize]
    [Route("/orders-info")]
    public OrdersInfoDto GetOrdersInfo() =>
        orderManager.GetOrdersInfo();
        
    [HttpGet]
    [Route("/orders/{OneTimeHash}")]
    public OrderGetSingleDto GetSingle([FromRoute]GetSingleOrderRequest request) =>
        orderManager.GetSingle(request);
}