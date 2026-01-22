using FluentValidation;
using LSCore.Validation.Domain;
using TD.Office.Public.Contracts.Requests.Ticket;

namespace TD.Office.Public.Domain.Validators.Ticket
{
	public class SaveTicketRequestValidator : LSCoreValidatorBase<SaveTicketRequest>
	{
		public SaveTicketRequestValidator()
		{
			RuleFor(x => x.Title)
				.NotEmpty()
				.WithMessage("Naslov je obavezno polje")
				.MaximumLength(200)
				.WithMessage("Naslov ne sme biti duži od 200 karaktera");

			RuleFor(x => x.Description)
				.NotEmpty()
				.WithMessage("Opis je obavezno polje")
				.MaximumLength(4000)
				.WithMessage("Opis ne sme biti duži od 4000 karaktera");

			RuleFor(x => x.Type)
				.IsInEnum()
				.WithMessage("Tip tiketa nije validan");
		}
	}
}
