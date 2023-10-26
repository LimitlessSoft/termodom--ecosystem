using FluentValidation;
using TD.Core.Contracts.Enums.ValidationCodes;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.ProductPriceGroup;
using TD.Web.Admin.Repository;

namespace TD.Web.Admin.Domain.Validators.ProductsPricesGroup
{
    public class ProductPriceGroupSaveRequestValidator : ValidatorBase<ProductPriceGroupSaveRequest>
    {
        private readonly int _maximumNameLength = 32;
        private readonly int _minimumNameLength = 6;
        public ProductPriceGroupSaveRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage(string.Format(CommonValidationCodes.COMM_002.GetDescription(String.Empty),nameof(ProductPriceGroupSaveRequest.Name)))
                .NotNull()
                    .WithMessage(string.Format(CommonValidationCodes.COMM_002.GetDescription(String.Empty), nameof(ProductPriceGroupSaveRequest.Name)))
                .MinimumLength(_minimumNameLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_003.GetDescription(String.Empty), nameof(ProductPriceGroupSaveRequest.Name), _minimumNameLength))
                .MaximumLength(_maximumNameLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_004.GetDescription(String.Empty), nameof(ProductPriceGroupSaveRequest.Name), _maximumNameLength))
                .Custom((name, context) =>
                {
                    var entity = dbContext.ProductPriceGroups.FirstOrDefault(x => x.Name == name);
                    if(entity != null)
                    {
                        context.AddFailure(ProductsPricesGroupValidationCodes.PPGVC_001.GetDescription(String.Empty));
                    }
                });
        }
    }
}
