using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Requests.Products;

namespace TD.Web.Public.Domain.Validators.Products;

public class AddToCartRequestValidator : LSCoreValidatorBase<AddToCartRequest>
{
	public AddToCartRequestValidator(IWebDbContextFactory dbContextFactory)
	{
		RuleFor(x => x.Id)
			.Custom(
				(id, context) =>
				{
					if (
						!dbContextFactory
							.Create<WebDbContext>()
							.Products.Any(x => x.Id == id && x.IsActive)
					)
						context.AddFailure(OrderItemsValidationCodes.OIVC_001.GetDescription());
				}
			);

		RuleFor(x => x.Quantity)
			.NotEmpty()
			.WithMessage(OrderItemsValidationCodes.OIVC_003.GetDescription());

		RuleFor(x => x)
			.Custom(
				(request, context) =>
				{
					var product = dbContextFactory
						.Create<WebDbContext>()
						.Products.FirstOrDefault(x => x.IsActive && x.Id == request.Id);
					if (product == null)
					{
						context.AddFailure(OrderItemsValidationCodes.OIVC_001.GetDescription());
						return;
					}

					if (
						product.OneAlternatePackageEquals is > 0
						&& request.Quantity % product.OneAlternatePackageEquals != 0
					)
						context.AddFailure(OrderItemsValidationCodes.OIVC_003.GetDescription());
				}
			);
	}
}
