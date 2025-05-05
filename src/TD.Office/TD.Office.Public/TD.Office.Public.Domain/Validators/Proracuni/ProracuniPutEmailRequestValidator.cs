using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Office.Public.Contracts.Enums.ValidationCodes;
using TD.Office.Public.Contracts.Requests.Proracuni;

namespace TD.Office.Public.Domain.Validators.Proracuni;

public class ProracuniPutEmailRequestValidator : LSCoreValidatorBase<ProracuniPutEmailRequest>
{
	public ProracuniPutEmailRequestValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.WithMessage(ProracuniValidationCodes.PVC_003.GetDescription())
			.EmailAddress()
			.WithMessage(ProracuniValidationCodes.PVC_004.GetDescription());
	}
}
