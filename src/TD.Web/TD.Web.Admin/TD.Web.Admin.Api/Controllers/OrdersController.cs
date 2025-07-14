using LSCore.Auth.Contracts;
using LSCore.SortAndPage.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Orders;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers;

[LSCoreAuth]
[ApiController]
[Permissions(Permission.Access)]
public class OrdersController(IOrderManager orderManager) : ControllerBase
{
	[HttpGet]
	[Route("/orders")]
	public LSCoreSortedAndPagedResponse<OrdersGetDto> GetMultiple(
		[FromQuery] OrdersGetMultipleRequest request
	) => orderManager.GetMultiple(request);

	[HttpGet]
	[Route("/orders/{OneTimeHash}")]
	public OrdersGetDto GetSingle([FromRoute] OrdersGetSingleRequest request) =>
		orderManager.GetSingle(request);

	[HttpPut]
	[Route("/orders/{OneTimeHash}/storeId/{StoreId}")]
	public void PutStoreId([FromRoute] OrdersPutStoreIdRequest request) =>
		orderManager.PutStoreId(request);

	[HttpPut]
	[Route("/orders/{OneTimeHash}/status/{status}")]
	public void PutStoreId([FromRoute] OrdersPutStatusRequest request) =>
		orderManager.PutStatus(request);

	[HttpPut]
	[Route("/orders/{OneTimeHash}/paymentTypeId/{PaymentTypeId}")]
	public void PutPaymentTypeId([FromRoute] OrdersPutPaymentTypeIdRequest request) =>
		orderManager.PutPaymentTypeId(request);

	[HttpPut]
	[Route("/orders/{OneTimeHash}/occupy-referent")]
	public void PutOccupyReferent([FromRoute] OrdersPutOccupyReferentRequest request) =>
		orderManager.PutOccupyReferent(request);

	[HttpPost]
	[Route("/orders/{OneTimeHash}/forward-to-komercijalno")]
	public Task PostForwardToKomercijalno(
		[FromRoute] string oneTimeHash,
		[FromBody] OrdersPostForwardToKomercijalnoRequest request
	)
	{
		request.OneTimeHash = oneTimeHash;
		return orderManager.PostForwardToKomercijalnoAsync(request);
	}

	[HttpPost]
	[Route("/orders/{OneTimeHash}/unlink-from-komercijalno")]
	public Task PostUnlinkFromKomercijalno(
		[FromRoute] OrdersPostUnlinkFromKomercijalnoRequest request
	) => orderManager.PostUnlinkFromKomercijalnoAsync(request);

	[HttpPut]
	[Route("/orders/{OneTimeHash}/admin-comment")]
	public void PutAdminComment(
		[FromRoute] string oneTimeHash,
		[FromBody] OrdersPutAdminCommentRequest request
	)
	{
		request.OneTimeHash = oneTimeHash;
		orderManager.PutAdminComment(request);
	}

	[HttpPut]
	[Route("/orders/{OneTimeHash}/public-comment")]
	public void PutPublicComment(
		[FromRoute] string oneTimeHash,
		[FromBody] OrdersPutPublicCommentRequest request
	)
	{
		request.OneTimeHash = oneTimeHash;
		orderManager.PutPublicComment(request);
	}
	
	[HttpPut]
	[Route("/orders/{OneTimeHash}/trgovac-action/{TrgovacAction}")]
	public void PutTrgovacAction(
		[FromRoute] OrdersPutTrgovacActionRequest request
	)
	{
		orderManager.PutTrgovacAction(request);
	}
}
