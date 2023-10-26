using FluentValidation;
using TD.Core.Contracts.Enums.ValidationCodes;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.ProductsGroups;
using TD.Web.Admin.Repository;

namespace TD.Web.Admin.Domain.Validators.ProductsGroups
{
    public class ProductsGroupsSaveRequestValidator : ValidatorBase<ProductsGroupsSaveRequest>
    {
        private readonly int _nameMaximumLength = 32;
        private readonly int _nameMinimumLength = 5;

        public ProductsGroupsSaveRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x)
                .Custom((request, context) =>
                {
                    var group = dbContext.ProductGroups.FirstOrDefault(x => x.Name == request.Name && x.IsActive);

                    if (request.IsOld && request.ParentGroupId.HasValue && request.Id == request.ParentGroupId)
                        context.AddFailure(ProductsGroupsValidationCodes.PGVC_003.GetDescription(String.Empty));
                    
                    if (group != null && group.Id != request.Id)
                        context.AddFailure(ProductsGroupsValidationCodes.PGVC_002.GetDescription(String.Empty));
                });

            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage(string.Format(CommonValidationCodes.COMM_002.GetDescription(String.Empty), nameof(ProductsGroupsSaveRequest.Name)))
                .MaximumLength(_nameMaximumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_003.GetDescription(String.Empty), nameof(ProductsGroupsSaveRequest.Name), _nameMaximumLength))
                .MinimumLength(_nameMinimumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_004.GetDescription(String.Empty), nameof(ProductsGroupsSaveRequest.Name), _nameMinimumLength));
            
            RuleFor(x => x.ParentGroupId)
                .Must(x => dbContext.ProductGroups.Any(z => z.Id == x && z.IsActive))
                    .WithMessage(ProductsGroupsValidationCodes.PGVC_001.GetDescription(String.Empty))
                .When(x => x.ParentGroupId.HasValue);
        }
    }
}
