using FluentValidation;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.Statistics;

namespace TD.Web.Admin.Domain.Validators.Statistics
{
    public class ProductsStatisticsRequestValidator : LSCoreValidatorBase<ProductsStatisticsRequest>
    {
        public ProductsStatisticsRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            
            RuleFor(x => x.DateFromUtc)
                .NotEmpty()
                .WithMessage(ProductsStatisticsValidationCodes.PSVC_002.GetDescription());

            RuleFor(x => x.DateToUtc)
                .NotEmpty()
                .WithMessage(ProductsStatisticsValidationCodes.PSVC_003.GetDescription());

            RuleFor(x => x)
                .Must((request) => request.DateFromUtc < request.DateToUtc)
                .WithMessage(ProductsStatisticsValidationCodes.PSVC_001.GetDescription());
        }
    }
}