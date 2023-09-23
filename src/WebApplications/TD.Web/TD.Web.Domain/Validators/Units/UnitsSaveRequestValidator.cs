using FluentValidation;
using TD.Core.Contracts.Enums.ValidationCodes;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.Enums.ValidationCodes;
using TD.Web.Contracts.Helpers.Units;
using TD.Web.Contracts.Requests.Units;
using TD.Web.Repository;

namespace TD.Web.Domain.Validators.Units
{
    public class UnitsSaveRequestValidator : ValidatorBase<UnitSaveRequest>
    {
        private const int NameMaximumLength = 32;
        private const int NameMinimumLength = 3;
        public UnitsSaveRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .Custom((id, context) =>
                {
                    var product = dbContext.Products.FirstOrDefault(x => x.Id == id);
                    if (product == null)
                        context.AddFailure(UnitsValidationCodes.UVC_001.GetDescription(String.Empty));
                })
                .When(x => x.Id.HasValue);
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(NameMaximumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_003.GetDescription(String.Empty), nameof(UnitSaveRequest.Name), NameMaximumLength))
                .MinimumLength(NameMinimumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_004.GetDescription(String.Empty), nameof(UnitSaveRequest.Name), NameMinimumLength))
                .Custom((name, context) =>
                {
                    if(name.IsNameNotValid())
                    {
                        context.AddFailure(string.Format(UnitsValidationCodes.UVC_002.GetDescription(String.Empty), nameof(UnitSaveRequest.Name)));
                    }
                });
        }
    }
}
