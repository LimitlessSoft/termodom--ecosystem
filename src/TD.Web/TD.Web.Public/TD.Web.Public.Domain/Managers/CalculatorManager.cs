using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Public.Contracts.Dtos.Calculator;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Calculator;

namespace TD.Web.Public.Domain.Managers;

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
				ProductName = calculatorItem.Product.Name,
				Quantity = calculatorItem.Quantity,
				Unit = calculatorItem.Product.Unit.Name,
				IsPrimary = calculatorItem.IsPrimary
			})
			.ToList();
	}

	public CalculatorDto GetCalculator(GetCalculatorRequest request)
	{
		var calculatorItems = calculatorItemRepository
			.GetMultiple()
			.Where(x => x.CalculatorType == request.Type)
			.Include(x => x.Product)
			.ThenInclude(x => x.Unit)
			.Include(x => x.Product)
			.ThenInclude(x => x.Price)
			.ToList();

		var hobiItems = calculatorItems.Where(x => x.IsHobi).ToList();
		var hobiWithoutDiscount = hobiItems.Sum(x =>
			x.Product.Price.Max * x.Quantity * request.Quantity
		);

		var standardItems = calculatorItems.Where(x => x.IsStandard).ToList();
		var standardWithoutDiscount = standardItems.Sum(x =>
			x.Product.Price.Max * x.Quantity * request.Quantity
		);

		var profiItems = calculatorItems.Where(x => x.IsProfi).ToList();
		var profiWithoutDiscount = profiItems.Sum(x =>
			x.Product.Price.Max * x.Quantity * request.Quantity
		);

		return new CalculatorDto()
		{
			// HobiValueWithVAT = hobiItems.Sum(item =>
			//     PricesHelpers.CalculateOneTimeCartPrice(
			//         item.Product.Price.Min,
			//         item.Product.Price.Max,
			//         hobiWithoutDiscount
			//     )
			//     * item.Quantity
			//     * request.Quantity
			// ),
			StandardValueWithVAT = standardItems.Sum(item =>
				PricesHelpers.CalculateOneTimeCartPrice(
					item.Product.Price.Min,
					item.Product.Price.Max,
					standardWithoutDiscount
				)
				* item.Quantity
				* request.Quantity
			),
			// ProfiValueWithVAT = profiItems.Sum(item =>
			//     PricesHelpers.CalculateOneTimeCartPrice(
			//         item.Product.Price.Min,
			//         item.Product.Price.Max,
			//         profiWithoutDiscount
			//     )
			//     * item.Quantity
			//     * request.Quantity
			// ),
		};
	}
}
