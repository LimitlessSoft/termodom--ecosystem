using FluentValidation;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using Microsoft.AspNetCore.Http;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Enums.ValidationCodes;
using TD.Web.Public.Contracts.Requests.Cart;

namespace TD.Web.Public.Domain.Validators.Cart
{
    public class CheckoutRequestValidator : LSCoreValidatorBase<CheckoutRequest>
    {
        private readonly Int16 _usernameMinimumLength = 6;
        private readonly Int16 _usernameMaximumLength = 32;
        private readonly Int16 _mobileMaximumLength = 16;
        private readonly Int16 _noteMaximumLength = 512;
        public CheckoutRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x)
                .Custom((request, context) =>
                {
                    if(!request.CurrentUser)
                    {
                        if (request.Mobile == null)
                            context.AddFailure(String.Format(CartValidationCodes.CVC_005.GetDescription(), nameof(CheckoutRequest.Mobile)));
                        if (request.Name == null)
                            context.AddFailure(String.Format(CartValidationCodes.CVC_005.GetDescription(), nameof(CheckoutRequest.Name)));
                    }
                });
            RuleFor(x => x.Note)
                .MaximumLength(_noteMaximumLength)
                    .WithMessage(String.Format(CartValidationCodes.CVC_003.GetDescription(), nameof(CheckoutRequest.Note), _noteMaximumLength));
            RuleFor(x => x.StoreId)
                .NotNull()
                .Custom((storeId, context) =>
                {
                    if(!dbContext.Stores.Any(y => y.Id == storeId && y.IsActive))
                        context.AddFailure(CartValidationCodes.CVC_001.GetDescription());
                });
            RuleFor(x => x.PaymentTypeId)
                .NotNull()
                .Custom((paymentTypeId, context) =>
                {
                    if (!dbContext.PaymentTypes.Any(y => y.Id == paymentTypeId && y.IsActive))
                        context.AddFailure(CartValidationCodes.CVC_004.GetDescription());
                });
            RuleFor(x => x.Name)
                .MaximumLength(_usernameMaximumLength)
                    .WithMessage(String.Format(CartValidationCodes.CVC_002.GetDescription(), nameof(CheckoutRequestBase.Name), _usernameMaximumLength))
                .Custom((name, context) =>
                {
                    if (name != null && name.Length < _usernameMinimumLength)
                        context.AddFailure(String.Format(CartValidationCodes.CVC_003.GetDescription(), nameof(CheckoutRequestBase.Name), _usernameMinimumLength));
                });
            RuleFor(x => x.Mobile)
                .MaximumLength(_mobileMaximumLength)
                    .WithMessage(String.Format(CartValidationCodes.CVC_002.GetDescription(), nameof(CheckoutRequestBase.Mobile), _mobileMaximumLength));
        }
    }
}
