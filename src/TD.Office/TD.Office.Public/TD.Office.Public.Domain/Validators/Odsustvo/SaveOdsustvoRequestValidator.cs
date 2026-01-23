using FluentValidation;
using LSCore.Validation.Domain;
using TD.Office.Public.Contracts.Requests.Odsustvo;

namespace TD.Office.Public.Domain.Validators.Odsustvo
{
	public class SaveOdsustvoRequestValidator : LSCoreValidatorBase<SaveOdsustvoRequest>
	{
		public SaveOdsustvoRequestValidator()
		{
			RuleFor(x => x.TipOdsustvaId)
				.GreaterThan(0)
				.WithMessage("Tip odsustva je obavezno polje");

			RuleFor(x => x.DatumOd)
				.NotEmpty()
				.WithMessage("Datum od je obavezno polje");

			RuleFor(x => x.DatumDo)
				.NotEmpty()
				.WithMessage("Datum do je obavezno polje");

			RuleFor(x => x)
				.Must(x => x.DatumDo >= x.DatumOd)
				.WithMessage("Datum do mora biti veći ili jednak datumu od");

			RuleFor(x => x.Komentar)
				.NotEmpty()
				.WithMessage("Opis je obavezno polje")
				.MaximumLength(500)
				.WithMessage("Opis ne sme biti duži od 500 karaktera");
		}
	}
}
