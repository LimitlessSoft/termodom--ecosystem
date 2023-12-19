using FluentValidation;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Requests.Products;
namespace TD.Web.Public.Domain.Validators.Products
{
    public class AddToCartRequestValidator : LSCoreValidatorBase<AddToCartRequest>
    {
        public AddToCartRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .Custom((id, context) =>
                {
                    if (!dbContext.Products.Any(x => x.Id == id && x.IsActive))
                        context.AddFailure(OrderItemsValidationCodes.OIVC_001.GetDescription());
                });
        }
    }
}
