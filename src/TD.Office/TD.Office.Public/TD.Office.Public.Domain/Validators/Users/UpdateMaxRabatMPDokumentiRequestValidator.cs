using FluentValidation;
using LSCore.Domain.Validators;
using TD.Office.Public.Contracts;
using TD.Office.Public.Contracts.Requests.Users;

namespace TD.Office.Public.Domain.Validators.Users;

public class UpdateMaxRabatMPDokumentiRequestValidator
    : LSCoreValidatorBase<UpdateMaxRabatMPDokumentiRequest>
{
    public UpdateMaxRabatMPDokumentiRequestValidator()
    {
        RuleFor(x => x.MaxRabatMPDokumenti)
            .GreaterThanOrEqualTo(LegacyConstants.MinRabatVPDokumenti)
            .LessThanOrEqualTo(LegacyConstants.MaxRabatMPDokumenti);
    }
}
