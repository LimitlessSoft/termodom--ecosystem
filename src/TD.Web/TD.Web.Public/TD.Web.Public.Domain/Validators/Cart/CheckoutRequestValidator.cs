using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Enums.ValidationCodes;
using TD.Web.Public.Contracts.Requests.Cart;

namespace TD.Web.Public.Domain.Validators.Cart;

public class CheckoutRequestValidator : LSCoreValidatorBase<CheckoutRequest>
{
	private readonly Int16 _usernameMinimumLength = 6;
	private readonly Int16 _usernameMaximumLength = 32;
	private readonly Int16 _mobileMaximumLength = 16;
	private readonly Int16 _noteMaximumLength = 512;

	public CheckoutRequestValidator(IWebDbContextFactory dbContextFactory)
	{
		RuleFor(x => x)
			.Custom(
				(request, context) =>
				{
					if (!request.IsCurrentUserAuthenticated)
					{
						if (string.IsNullOrEmpty(request.Mobile))
							context.AddFailure(
								String.Format(
									CartValidationCodes.CVC_005.GetDescription(),
									nameof(CheckoutRequest.Mobile)
								)
							);
						if (string.IsNullOrEmpty(request.Name))
							context.AddFailure(
								String.Format(
									CartValidationCodes.CVC_005.GetDescription(),
									nameof(CheckoutRequest.Name)
								)
							);
					}
				}
			);
		RuleFor(x => x.Note)
			.MaximumLength(_noteMaximumLength)
			.WithMessage(
				String.Format(CartValidationCodes.CVC_003.GetDescription(), _noteMaximumLength)
			);
		RuleFor(x => x.StoreId)
			.NotNull()
			.Custom(
				(storeId, context) =>
				{
					using var dbContext = dbContextFactory.Create<WebDbContext>();
					if (!dbContext.Stores.Any(y => y.Id == storeId && y.IsActive))
						context.AddFailure(CartValidationCodes.CVC_001.GetDescription());
				}
			);
		RuleFor(x => x.PaymentTypeId)
			.NotNull()
			.Custom(
				(paymentTypeId, context) =>
				{
					using var dbContext = dbContextFactory.Create<WebDbContext>();
					if (!dbContext.PaymentTypes.Any(y => y.Id == paymentTypeId && y.IsActive))
						context.AddFailure(CartValidationCodes.CVC_004.GetDescription());
				}
			);
		RuleFor(x => x.Name)
			.MaximumLength(_usernameMaximumLength)
			.WithMessage(
				String.Format(CartValidationCodes.CVC_002.GetDescription(), _usernameMaximumLength)
			)
			.Custom(
				(name, context) =>
				{
					if (name != null && name.Length < _usernameMinimumLength)
						context.AddFailure(
							String.Format(
								CartValidationCodes.CVC_006.GetDescription(),
								_usernameMinimumLength
							)
						);
				}
			);
		RuleFor(x => x.Mobile)
			.MaximumLength(_mobileMaximumLength)
			.WithMessage(
				String.Format(CartValidationCodes.CVC_007.GetDescription(), _mobileMaximumLength)
			);
	}
}
