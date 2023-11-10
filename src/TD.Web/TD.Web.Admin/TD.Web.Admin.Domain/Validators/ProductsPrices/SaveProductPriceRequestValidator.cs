using FluentValidation;
using LSCore.Domain.Validators;
using TD.Web.Admin.Contracts.Requests.ProductsPrices;

namespace TD.Web.Admin.Domain.Validators.ProductsPrices
{
    public class SaveProductPriceRequestValidator : LSCoreValidatorBase<SaveProductPriceRequest>
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
