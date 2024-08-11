using FluentValidation;
using LSCore.Contracts.Enums.ValidationCodes;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.ProductsGroups;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.ProductsGroups
{
    public class ProductsGroupsSaveRequestValidator : LSCoreValidatorBase<ProductsGroupsSaveRequest>
    {
        private readonly int _nameMaximumLength = 32;
        private readonly int _nameMinimumLength = 5;

        public ProductsGroupsSaveRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x)
                .Custom((request, context) =>
                {
                    var group = dbContext.ProductGroups.FirstOrDefault(x => x.Name.ToLower() == request.Name.ToLower() && x.IsActive);

                    if (request.IsOld && request.ParentGroupId.HasValue && request.Id == request.ParentGroupId)
                        context.AddFailure(ProductsGroupsValidationCodes.PGVC_003.GetDescription());
                    
                    if (group != null && group.Id != request.Id)
                        context.AddFailure(ProductsGroupsValidationCodes.PGVC_002.GetDescription());
                });

            RuleFor(x => x.Name)
            .NotEmpty()
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_002.GetDescription(), nameof(ProductsGroupsSaveRequest.Name)))
                .MaximumLength(_nameMaximumLength)
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_003.GetDescription(), nameof(ProductsGroupsSaveRequest.Name), _nameMaximumLength))
                .MinimumLength(_nameMinimumLength)
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_004.GetDescription(), nameof(ProductsGroupsSaveRequest.Name), _nameMinimumLength));
            
            RuleFor(x => x.ParentGroupId)
                .Must(x => dbContext.ProductGroups.Any(z => z.Id == x && z.IsActive))
                    .WithMessage(ProductsGroupsValidationCodes.PGVC_001.GetDescription())
                .When(x => x.ParentGroupId.HasValue);
        }
    }
}
