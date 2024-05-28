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

            RuleFor(x => x.Quantity)
                .NotEmpty()
                    .WithMessage(OrderItemsValidationCodes.OIVC_003.GetDescription());

            RuleFor(x => x)
                .Custom((request, context) =>
                {
                    var product = dbContext.Products.FirstOrDefault(x => x.IsActive && x.Id == request.Id);
                    if(product == null)
                        context.AddFailure(OrderItemsValidationCodes.OIVC_001.GetDescription());
                    
                    if(product!.OneAlternatePackageEquals != null && request.Quantity % product!.OneAlternatePackageEquals != 0)
                        context.AddFailure(OrderItemsValidationCodes.OIVC_003.GetDescription());
                });
        }
    }
}
