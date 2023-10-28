using FluentValidation;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.ProductsGroups;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.ProductsGroups
{
    public class ProductsGroupsDeleteRequestValidator : ValidatorBase<ProductsGroupsDeleteRequest>
    {
        public ProductsGroupsDeleteRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x)
                .Custom((request, context) =>
                {
                    if (dbContext.Products.Any(z => z.Groups.Any(k => k.Id == request.Id)))
                        context.AddFailure(ProductsGroupsValidationCodes.PGVC_005.GetDescription(String.Empty));
                });

            RuleFor(x => x.Id)
                .Must(x => !dbContext.ProductGroups.Any(z => z.ParentGroupId == x && z.IsActive))
                    .WithMessage(ProductsGroupsValidationCodes.PGVC_004.GetDescription(String.Empty));
        }
    }
}
