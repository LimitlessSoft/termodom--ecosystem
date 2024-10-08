using LSCore.Contracts;
using LSCore.Contracts.IManagers;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Dtos.Calculator;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Calculator;

namespace TD.Web.Public.Domain.Managers;

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
                ProductName = calculatorItem.Product.Name,
                Quantity = calculatorItem.Quantity,
                Unit = calculatorItem.Product.Unit.Name,
                IsPrimary = calculatorItem.IsPrimary
            })
            .ToList();
    }
}
