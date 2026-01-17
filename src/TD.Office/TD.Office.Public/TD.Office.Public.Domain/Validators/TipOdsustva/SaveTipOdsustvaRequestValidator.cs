using FluentValidation;
using LSCore.Validation.Domain;
using TD.Office.Public.Contracts.Requests.TipOdsustva;

namespace TD.Office.Public.Domain.Validators.TipOdsustva
{
	public class SaveTipOdsustvaRequestValidator : LSCoreValidatorBase<SaveTipOdsustvaRequest>
	{
		public SaveTipOdsustvaRequestValidator()
		{
			RuleFor(x => x.Naziv)
				.NotEmpty()
				.WithMessage("Naziv je obavezno polje");

			RuleFor(x => x.Naziv)
				.MaximumLength(100)
				.WithMessage("Naziv ne sme biti duži od 100 karaktera");

			RuleFor(x => x.Redosled)
				.GreaterThanOrEqualTo(0)
				.WithMessage("Redosled mora biti pozitivan broj");
		}
	}
}
