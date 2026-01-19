using FluentValidation;
using LSCore.Validation.Domain;
using TD.Office.Public.Contracts.Requests.TipKorisnika;

namespace TD.Office.Public.Domain.Validators.TipKorisnika
{
	public class SaveTipKorisnikaRequestValidator : LSCoreValidatorBase<SaveTipKorisnikaRequest>
	{
		public SaveTipKorisnikaRequestValidator()
		{
			RuleFor(x => x.Naziv)
				.NotEmpty()
				.WithMessage("Naziv je obavezno polje");

			RuleFor(x => x.Naziv)
				.MaximumLength(100)
				.WithMessage("Naziv ne sme biti duži od 100 karaktera");

			RuleFor(x => x.Boja)
				.NotEmpty()
				.WithMessage("Boja je obavezno polje");

			RuleFor(x => x.Boja)
				.Matches("^#[0-9A-Fa-f]{6}$")
				.WithMessage("Boja mora biti u hex formatu (#RRGGBB)");
		}
	}
}
