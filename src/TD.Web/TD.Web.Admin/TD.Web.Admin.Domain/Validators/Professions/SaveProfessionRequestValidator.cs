using FluentValidation;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.Professions;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.Professions
{
    public class SaveProfessionRequestValidator : LSCoreValidatorBase<SaveProfessionRequest>
    {
        private readonly Int16 _nameMaximumLength = 32;
        private readonly Int16 _nameMinimumLength = 3;
        public SaveProfessionRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x.Name)
                .NotNull()
                    .WithMessage(ProfessionValidationCodes.PVC_001.GetDescription())
                .MaximumLength(_nameMaximumLength)
                    .WithMessage(string.Format(ProfessionValidationCodes.PVC_002.GetDescription(), _nameMaximumLength))
                .MinimumLength(_nameMinimumLength)
                    .WithMessage(string.Format(ProfessionValidationCodes.PVC_003.GetDescription(), _nameMinimumLength))
                .Custom((name, context) =>
                {
                    if(dbContext.Professions.Any(x => x.IsActive && string.Equals(x.Name.ToLower(), name.ToLower())))
                        context.AddFailure(ProfessionValidationCodes.PVC_004.GetDescription());
                });
        }
    }
}
