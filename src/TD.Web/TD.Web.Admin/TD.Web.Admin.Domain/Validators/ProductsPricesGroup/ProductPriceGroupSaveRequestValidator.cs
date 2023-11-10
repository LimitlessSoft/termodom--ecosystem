using FluentValidation;
using LSCore.Contracts.Enums.ValidationCodes;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.ProductPriceGroup;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.ProductsPricesGroup
{
    public class ProductPriceGroupSaveRequestValidator : LSCoreValidatorBase<ProductPriceGroupSaveRequest>
    {
        private readonly int _maximumNameLength = 32;
        private readonly int _minimumNameLength = 6;
        public ProductPriceGroupSaveRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_002.GetDescription(),nameof(ProductPriceGroupSaveRequest.Name)))
                .NotNull()
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_002.GetDescription(), nameof(ProductPriceGroupSaveRequest.Name)))
                .MinimumLength(_minimumNameLength)
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_003.GetDescription(), nameof(ProductPriceGroupSaveRequest.Name), _minimumNameLength))
                .MaximumLength(_maximumNameLength)
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_004.GetDescription(), nameof(ProductPriceGroupSaveRequest.Name), _maximumNameLength))
                .Custom((name, context) =>
                {
                    var entity = dbContext.ProductPriceGroups.FirstOrDefault(x => x.Name == name);
                    if(entity != null)
                    {
                        context.AddFailure(ProductsPricesGroupValidationCodes.PPGVC_001.GetDescription());
                    }
                });
        }
    }
}
