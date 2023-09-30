using FluentValidation;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.Requests.ProductsPrices;

namespace TD.Web.Domain.Validators.ProductsPrices
{
    public class SaveProductPriceRequestValidator : ValidatorBase<SaveProductPriceRequest>
    {
        public SaveProductPriceRequestValidator()
        {
            RuleFor(x => x.Min)
                .NotEmpty()
                .LessThanOrEqualTo(x => x.Max);

            RuleFor(x => x.Max)
                .NotEmpty()
                .GreaterThanOrEqualTo(x => x.Min);
        }
    }
}
