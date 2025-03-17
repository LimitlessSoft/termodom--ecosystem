using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Requests.OrderItems;
using TD.Web.Common.Repository;

namespace TD.Web.Public.Domain.Validators.OrderItems;

public class ChangeOrderItemQuantityRequestValidator
	: LSCoreValidatorBase<ChangeOrderItemQuantityRequest>
{
	private readonly Int16 _quantityMinimumValue = 0;

	public ChangeOrderItemQuantityRequestValidator(WebDbContext dbContext)
	{
		RuleFor(x => x)
			.Custom(
				(x, context) =>
				{
					if (
						!dbContext.OrderItems.Any(c =>
							c.IsActive && c.OrderId == x.OrderId && c.ProductId == x.ProductId
						)
					)
						context.AddFailure(OrderItemsValidationCodes.OIVC_002.GetDescription());
				}
			);
		RuleFor(x => x.Quantity).NotNull().GreaterThan(_quantityMinimumValue);
	}
}
