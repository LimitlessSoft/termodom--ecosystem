using FluentValidation;
using TD.Core.Contracts.Enums.ValidationCodes;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.Enums.ValidationCodes;
using TD.Web.Contracts.Requests.ProductsGroups;
using TD.Web.Repository;

namespace TD.Web.Domain.Validators.ProductsGroups
{
    public class ProductsGroupsSaveRequestValidator : ValidatorBase<ProductsGroupsSaveRequest>
    {
        private const int NameMaximumLength = 32;
        private const int NameMinimumLength = 5;

        public ProductsGroupsSaveRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x)
                .Custom((request, context) =>
                {

                    var group = dbContext.ProductGroups.FirstOrDefault(x => x.Name == request.Name && x.IsActive);
                    if (request.Id != null && request.ParentGroupId != null && request.Id == request.ParentGroupId)
                    {
                        context.AddFailure(ProductsGroupsValidationCodes.PGVC_003.GetDescription(String.Empty));
                    }
                    if (group != null && group.Id != request.Id)
                    {
                        context.AddFailure(ProductsGroupsValidationCodes.PGVC_002.GetDescription(String.Empty));
                    }
                });
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .MaximumLength(NameMaximumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_003.GetDescription(String.Empty), nameof(ProductsGroupsSaveRequest.Name), NameMaximumLength))
                .MinimumLength(NameMinimumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_004.GetDescription(String.Empty), nameof(ProductsGroupsSaveRequest.Name), NameMinimumLength));
            RuleFor(x => x.ParentGroupId)
                .Custom((parentGroupId, context) =>
                {
                    if(parentGroupId != null)
                    {
                        var group = dbContext.ProductGroups.FirstOrDefault(x => x.Id == parentGroupId && x.IsActive);
                        if (group == null)
                            context.AddFailure(ProductsGroupsValidationCodes.PGVC_001.GetDescription(String.Empty));
                    }
                });
        }
    }
}
