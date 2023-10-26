using FluentValidation;
using TD.Core.Contracts.Enums.ValidationCodes;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Helpers.Units;
using TD.Web.Admin.Contracts.Requests.Units;
using TD.Web.Admin.Repository;

namespace TD.Web.Admin.Domain.Validators.Units
{
    public class UnitsSaveRequestValidator : ValidatorBase<UnitSaveRequest>
    {
        private const int NameMaximumLength = 32;
        private const int NameMinimumLength = 3;
        public UnitsSaveRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage(string.Format(CommonValidationCodes.COMM_002.GetDescription(String.Empty), nameof(UnitSaveRequest.Name)))
                .MaximumLength(NameMaximumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_003.GetDescription(String.Empty), nameof(UnitSaveRequest.Name), NameMaximumLength))
                .MinimumLength(NameMinimumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_004.GetDescription(String.Empty), nameof(UnitSaveRequest.Name), NameMinimumLength))
                .Custom((name, context) =>
                {
                    if(name.IsNameNotValid())
                        context.AddFailure(string.Format(UnitsValidationCodes.UVC_002.GetDescription(String.Empty), nameof(UnitSaveRequest.Name)));

                    var unit = dbContext.Units.FirstOrDefault(x => x.Name == name.NormalizeName());
                    if(unit != null)
                        context.AddFailure(string.Format(UnitsValidationCodes.UVC_001.GetDescription(String.Empty), nameof(UnitSaveRequest.Name)));

                });
        }
    }
}
