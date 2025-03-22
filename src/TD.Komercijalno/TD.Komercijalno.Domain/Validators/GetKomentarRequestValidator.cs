using FluentValidation;
using LSCore.Validation.Domain;
using TD.Komercijalno.Contracts.Requests.Komentari;

namespace TD.Komercijalno.Domain.Validators;

public class GetKomentarRequestValidator : LSCoreValidatorBase<GetKomentarRequest>
{
	public GetKomentarRequestValidator()
	{
		RuleFor(x => x.VrDok).NotNull();

		RuleFor(x => x.BrDok).NotEmpty();
	}
}
