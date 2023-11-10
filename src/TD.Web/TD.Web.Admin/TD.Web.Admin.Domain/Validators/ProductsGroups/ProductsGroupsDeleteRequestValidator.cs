using FluentValidation;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.ProductsGroups;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.ProductsGroups
{
    public class ProductsGroupsDeleteRequestValidator : LSCoreValidatorBase<ProductsGroupsDeleteRequest>
    {
        public ProductsGroupsDeleteRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x)
                .Must(x => !dbContext.Products.Any(z => z.Groups.Any(k => k.Id == x.Id)))
                    .WithMessage(ProductsGroupsValidationCodes.PGVC_005.GetDescription());

            RuleFor(x => x.Id)
                .Must(x => !dbContext.ProductGroups.Any(z => z.ParentGroupId == x && z.IsActive))
                    .WithMessage(ProductsGroupsValidationCodes.PGVC_004.GetDescription());
        }
    }
}
