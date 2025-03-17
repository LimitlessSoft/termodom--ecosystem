using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Komercijalno.Contracts.Requests.Partneri;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.Enums.ValidationCodes;
using TD.Office.Common.Contracts.Models;

namespace TD.Office.Public.Domain.Validators.Partners
{
	public class PartneriCreateRequestValidator : LSCoreValidatorBase<PartneriCreateRequest>
	{
		private int _nazivMaximumLength = 50;
		private int _adresaMaximumLength = 250;
		private int _postaMaximumLength = 10;
		private int _mestoMaximumLength = 20;
		private int _emailMaximumLength = 180;
		private int _kontaktMaximumLength = 50;
		private int _mbrojMaximumLength = 20;
		private int _mestoIdMaximumLength = 5;
		private int _pibMaximumLength = 20;
		private int _mobilniMaximumLength = 50;

		public PartneriCreateRequestValidator()
		{
			RuleFor(x => x.Naziv)
				.NotEmpty()
				.MaximumLength(_nazivMaximumLength)
				.Custom(
					(naziv, context) =>
					{
						if (
							!Enum.GetValues(typeof(CompanyType))
								.Cast<CompanyType>()
								.Any(e =>
									naziv.EndsWith(e.ToString(), StringComparison.OrdinalIgnoreCase)
								)
						)
							context.AddFailure(PartnersValidationCodes.PVC_001.GetDescription());
					}
				);

			RuleFor(x => x.Adresa).NotEmpty().MaximumLength(_adresaMaximumLength);

			RuleFor(x => x.Posta).NotEmpty().MaximumLength(_postaMaximumLength);

			RuleFor(x => x.Mesto).NotEmpty().MaximumLength(_mestoMaximumLength);

			RuleFor(x => x.Email).NotEmpty().MaximumLength(_emailMaximumLength);

			RuleFor(x => x.Kontakt).NotEmpty().MaximumLength(_kontaktMaximumLength);

			RuleFor(x => x.Mbroj).NotEmpty().MaximumLength(_mbrojMaximumLength);

			RuleFor(x => x.MestoId).NotEmpty().MaximumLength(_mestoIdMaximumLength);

			RuleFor(x => x.Pib).NotEmpty().MaximumLength(_pibMaximumLength);

			RuleFor(x => x.Mobilni)
				.NotEmpty()
				.MaximumLength(_mobilniMaximumLength)
				.Custom(
					(m, context) =>
					{
						if (MobileNumber.IsValid(m))
							return;

						context.AddFailure("Mobilni nije ispravan!");
					}
				);
		}
	}
}
