using FluentValidation;
using LSCore.Domain.Validators;
using TD.Office.InterneOtpremnice.Contracts.Requests;

namespace TD.Office.InterneOtpremnice.Domain.Validators;

public class GetMultipleRequestValidator : LSCoreValidatorBase<GetMultipleRequest>
{
    public GetMultipleRequestValidator()
    {
        RuleFor(x => x.Vrsta).NotEmpty();
    }
}
