using FluentValidation;
using LSCore.Validation.Domain;
using TD.Komercijalno.Contracts.Requests.Dokument;

namespace TD.Komercijalno.Domain.Validators;

public class DokumentCreateRequestValidator : LSCoreValidatorBase<DokumentCreateRequest>
{
	public DokumentCreateRequestValidator()
		: base()
	{
		RuleFor(x => x.VrDok).NotEmpty();

		RuleFor(x => x.MagacinId).NotEmpty();

		RuleFor(x => x.ZapId).NotEmpty();

		RuleFor(x => x.RefId).NotEmpty();
	}
}
