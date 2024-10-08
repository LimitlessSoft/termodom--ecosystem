using LSCore.Contracts;
using LSCore.Contracts.IManagers;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Helpers;
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

    public CalculatorDto GetCalculator(GetCalculatorRequest request)
    {
        var calculatorItems = Queryable()
            .Where(x => x.CalculatorType == request.Type)
            .Include(x => x.Product)
            .ThenInclude(x => x.Unit)
            .Include(x => x.Product)
            .ThenInclude(x => x.Price)
            .Where(x => x.IsActive)
            .ToList();

        var hobiItems = calculatorItems
            .Where(x => x.Product.Classification == ProductClassification.Hobi)
            .ToList();
        var hobiWithoutDiscount = hobiItems.Sum(x =>
            x.Product.Price.Max * x.Quantity * request.Quantity
        );

        var standardItems = calculatorItems
            .Where(x => x.Product.Classification == ProductClassification.Standard)
            .ToList();
        var standardWithoutDiscount = standardItems.Sum(x =>
            x.Product.Price.Max * x.Quantity * request.Quantity
        );

        var profiItems = calculatorItems
            .Where(x => x.Product.Classification == ProductClassification.Profi)
            .ToList();
        var profiWithoutDiscount = profiItems.Sum(x =>
            x.Product.Price.Max * x.Quantity * request.Quantity
        );

        return new CalculatorDto()
        {
            HobiValueWithVAT = hobiItems.Sum(item =>
                PricesHelpers.CalculateOneTimeCartPrice(
                    item.Product.Price.Min,
                    item.Product.Price.Max,
                    hobiWithoutDiscount
                )
                * item.Quantity
                * request.Quantity
            ),
            StandardValueWithVAT = standardItems.Sum(item =>
                PricesHelpers.CalculateOneTimeCartPrice(
                    item.Product.Price.Min,
                    item.Product.Price.Max,
                    standardWithoutDiscount
                )
                * item.Quantity
                * request.Quantity
            ),
            ProfiValueWithVAT = profiItems.Sum(item =>
                PricesHelpers.CalculateOneTimeCartPrice(
                    item.Product.Price.Min,
                    item.Product.Price.Max,
                    profiWithoutDiscount
                )
                * item.Quantity
                * request.Quantity
            ),
        };
    }
}
