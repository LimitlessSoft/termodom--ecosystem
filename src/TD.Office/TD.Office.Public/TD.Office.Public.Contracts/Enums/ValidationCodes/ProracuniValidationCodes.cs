using System.ComponentModel;

namespace TD.Office.Public.Contracts.Enums.ValidationCodes;

public enum ProracuniValidationCodes
{
	[Description(
		"Ne mozete kreirati proracun tipa Nalog za utovar jer nemate povezan komercijalni nalog."
	)]
	PVC_001,

	[Description("Korisnik nema dodeljen {0} magacin")]
	PVC_002,

	[Description("Morate uneti email adresu.")]
	PVC_003,

	[Description("Email adresa nije validna.")]
	PVC_004,
}
