using LSCore.Exceptions;
using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Common.Contracts.Requests.OrderItems;

namespace TD.Web.Common.Domain.Managers;

public class OrderItemManager(IOrderItemRepository repository) : IOrderItemManager
{
	public OrderItemEntity Insert(OrderItemEntity request)
	{
		var entity = new OrderItemEntity();
		entity.InjectFrom(request);
		repository.Insert(entity);
		return entity;
	}

	public bool Exists(OrderItemExistsRequest request) =>
		repository
			.GetMultiple()
			.Where(x => x.ProductId == request.ProductId && x.OrderId == request.OrderId)
			.Include(x => x.Order)
			.Any();

	public void Delete(DeleteOrderItemRequest request)
	{
		request.Validate();
		repository.HardDelete(
			GetOrderItem(
				new GetOrderItemRequest()
				{
					OrderId = request.OrderId,
					ProductId = request.ProductId
				}
			)
		);
	}

	public OrderItemEntity GetOrderItem(GetOrderItemRequest request) =>
		repository
			.GetMultiple()
			.FirstOrDefault(x => x.OrderId == request.OrderId && x.ProductId == request.ProductId)
		?? throw new LSCoreNotFoundException();

	public void ChangeQuantity(ChangeOrderItemQuantityRequest request)
	{
		request.Validate();

		var item = GetOrderItem(
			new GetOrderItemRequest() { OrderId = request.OrderId, ProductId = request.ProductId }
		);
		item.Quantity = request.Quantity;
		repository.Update(item);
	}
}
