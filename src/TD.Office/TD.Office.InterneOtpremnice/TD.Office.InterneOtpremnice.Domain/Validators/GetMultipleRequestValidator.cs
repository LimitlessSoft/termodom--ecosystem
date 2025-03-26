using FluentValidation;
using LSCore.Validation.Domain;
using TD.Office.InterneOtpremnice.Contracts.Requests;

namespace TD.Office.InterneOtpremnice.Domain.Validators;

public class GetMultipleRequestValidator : LSCoreValidatorBase<GetMultipleRequest>
{
	public GetMultipleRequestValidator()
	{
		RuleFor(x => x.Vrsta).NotNull();
	}
}
