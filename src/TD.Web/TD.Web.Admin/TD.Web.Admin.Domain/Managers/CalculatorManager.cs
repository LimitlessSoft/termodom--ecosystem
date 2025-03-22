using LSCore.Common.Contracts;
using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.Calculator;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Calculator;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Admin.Domain.Managers;

public class CalculatorManager(ICalculatorItemRepository calculatorItemRepository)
	: ICalculatorManager
{
	public List<CalculatorItemDto> GetCalculatorItems(GetCalculatorItemsRequest request)
	{
		request.Validate();

		var calculatorItems = calculatorItemRepository
			.GetMultiple()
			.Where(x => x.CalculatorType == request.Type)
			.Include(x => x.Product)
			.ThenInclude(x => x.Unit)
			.ToList();

		return calculatorItems
			.Select(calculatorItem => new CalculatorItemDto
			{
				Id = calculatorItem.Id,
				ProductName = calculatorItem.Product.Name,
				Quantity = calculatorItem.Quantity,
				Unit = calculatorItem.Product.Unit.Name,
				CalculatorType = calculatorItem.CalculatorType,
				Order = calculatorItem.Order,
				IsPrimary = calculatorItem.IsPrimary,
				IsHobi = calculatorItem.IsHobi,
				IsStandard = calculatorItem.IsStandard,
				IsProfi = calculatorItem.IsProfi
			})
			.ToList();
	}

	public void AddCalculatorItem(AddCalculatorItemRequest request)
	{
		var maxCalculatorItemOrderFromThisGroup = calculatorItemRepository
			.GetMultiple()
			.Where(x => x.CalculatorType == request.CalculatorType && x.IsActive)
			.Max(x => x.Order);
		var calculatorItem = new CalculatorItemEntity();
		calculatorItem.InjectFrom(request);
		calculatorItem.Order = maxCalculatorItemOrderFromThisGroup + 1;
		calculatorItem.IsHobi = false;
		calculatorItem.IsStandard = true;
		calculatorItem.IsProfi = false;
		calculatorItemRepository.Insert(calculatorItem);
	}

	public void RemoveCalculatorItem(RemoveCalculatorItemRequest request) =>
		calculatorItemRepository.HardDelete(request.Id);

	public void UpdateCalculatorItemQuantity(UpdateCalculatorItemQuantityRequest request)
	{
		var entity = request.Id.HasValue
			? calculatorItemRepository.Get(request.Id.Value)
			: new CalculatorItemEntity();

		entity.InjectFrom(request);
		calculatorItemRepository.UpdateOrInsert(entity);
	}

	public void MarkAsPrimaryCalculatorItem(LSCoreIdRequest request)
	{
		var calculatorItem = calculatorItemRepository.Get(request.Id);

		var itemsFromGroup = calculatorItemRepository
			.GetMultiple()
			.Where(x => x.CalculatorType == calculatorItem.CalculatorType)
			.ToList();

		foreach (var item in itemsFromGroup)
		{
			item.IsPrimary = item.Id == request.Id;
			calculatorItemRepository.Update(item);
		}
	}

	public void UpdateCalculatorItemUnit(UpdateCalculatorItemUnitRequest request)
	{
		throw new NotImplementedException();
	}

	public void MoveCalculatorItem(MoveCalculatorItemRequest request)
	{
		var calculatorItem = calculatorItemRepository.Get(request.Id);

		var itemsFromGroup = calculatorItemRepository
			.GetMultiple()
			.Where(x => x.CalculatorType == calculatorItem.CalculatorType)
			.OrderBy(x => x.Order)
			.ToList();

		var newOrderItems = new List<CalculatorItemEntity>();
		var order = request.Direction == "up" ? -1 : 1;
		var bufferUp = false;
		foreach (var item in itemsFromGroup)
		{
			if (bufferUp)
			{
				item.Order -= order;
				bufferUp = false;
				newOrderItems.Add(item);
				break;
			}
			if (item.Id == request.Id)
			{
				item.Order += order;

				if (request.Direction == "up")
					newOrderItems.Last().Order += 1;
				else
					bufferUp = true;
			}

			newOrderItems.Add(item);
		}

		foreach (var item in newOrderItems)
			calculatorItemRepository.Update(item);
	}

	public void DeleteCalculatorItem(LSCoreIdRequest request) =>
		calculatorItemRepository.HardDelete(request.Id);

	public void UpdateCalculatorItemClassification(
		UpdateCalculatorItemClassificationRequest request
	)
	{
		var calculatorItem = calculatorItemRepository.Get(request.Id);

		calculatorItem.IsHobi = request.IsHobi;
		calculatorItem.IsStandard = request.IsStandard;
		calculatorItem.IsProfi = request.IsProfi;

		calculatorItemRepository.Update(calculatorItem);
	}
}
