using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.Calculator;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Calculator;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Managers;

public class CalculatorManager(
    ILogger<CalculatorManager> logger,
    WebDbContext dbContext,
    LSCoreContextUser currentUser
)
    : LSCoreManagerBase<CalculatorManager, CalculatorItemEntity>(logger, dbContext, currentUser),
        ICalculatorManager
{
    public List<CalculatorItemDto> GetCalculatorItems(GetCalculatorItemsRequest request)
    {
        request.Validate();

        var calculatorItems = Queryable()
            .Where(x => x.CalculatorType == request.Type)
            .Include(x => x.Product)
            .ThenInclude(x => x.Unit)
            .Where(x => x.IsActive)
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
                IsPrimary = calculatorItem.IsPrimary
            })
            .ToList();
    }

    public void AddCalculatorItem(AddCalculatorItemRequest request)
    {
        var maxCalculatorItemOrderFromThisGroup = Queryable()
            .Where(x => x.CalculatorType == request.CalculatorType && x.IsActive)
            .Max(x => x.Order);
        var calculatorItem = new CalculatorItemEntity();
        calculatorItem.InjectFrom(request);
        calculatorItem.Order = maxCalculatorItemOrderFromThisGroup + 1;
        Insert(calculatorItem);
    }

    public void RemoveCalculatorItem(RemoveCalculatorItemRequest request) => HardDelete(request);

    public void UpdateCalculatorItemQuantity(UpdateCalculatorItemQuantityRequest request) =>
        Save(request);

    public void MarkAsPrimaryCalculatorItem(LSCoreIdRequest request)
    {
        var calculatorItem = Queryable().FirstOrDefault(x => x.Id == request.Id && x.IsActive);
        if (calculatorItem == null)
            throw new LSCoreNotFoundException();

        var itemsFromGroup = Queryable()
            .Where(x => x.CalculatorType == calculatorItem.CalculatorType && x.IsActive)
            .ToList();

        foreach (var item in itemsFromGroup)
        {
            item.IsPrimary = item.Id == request.Id;
            Update(item);
        }
    }

    public void UpdateCalculatorItemUnit(UpdateCalculatorItemUnitRequest request) => Save(request);

    public void MoveCalculatorItem(MoveCalculatorItemRequest request)
    {
        var calculatorItem = Queryable().FirstOrDefault(x => x.Id == request.Id && x.IsActive);
        if (calculatorItem == null)
            throw new LSCoreNotFoundException();

        var itemsFromGroup = Queryable()
            .Where(x => x.CalculatorType == calculatorItem.CalculatorType && x.IsActive)
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
            Update(item);
    }

    public void DeleteCalculatorItem(LSCoreIdRequest request) => HardDelete(request.Id);
}
