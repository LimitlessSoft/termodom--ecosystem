using FluentValidation;
using TD.Core.Domain.Validators;
using TD.Web.Veleprodaja.Contracts.Requests.Products;

namespace TD.Web.Veleprodaja.Domain.Validators
{
    public class ProductsPutRequestValidator : ValidatorBase<ProductsPutRequest>
    {
        public ProductsPutRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(32)
                .MinimumLength(6);

            RuleFor(x => x.Unit)
                .NotEmpty()
                .MaximumLength(8)
                .MinimumLength(2);

            RuleFor(x => x.ThumbnailImagePath)
                .NotEmpty();

            RuleFor(x => x.FullSizedImagePath)
                .NotEmpty();

            RuleFor(x => x.SKU)
                .NotEmpty();
        }
    }
}
