using LSCore.Validation.Contracts;

namespace TD.Office.MassSMS.Contracts.Enums.ValidationCodes;

public enum SMSValidationCodes
{
	[LSCoreValidationMessage(
		"Poruka sadrzi nepodrzane karaktere. Samo ASCII karakteri su dozvoljeni"
	)]
	SVC_001,

	[LSCoreValidationMessage(
		"Ne mozete dodati novu poruku u red slanja jer je trenutni status masovnog slanja {0}!"
	)]
	SVC_002,

	[LSCoreValidationMessage("Text poruke ne sme biti prazan")]
	SVC_003,

	[LSCoreValidationMessage("Poruka je predugacka. Maksimalna dozvoljena duzina je {0} karaktera")]
	SVC_004,

	[LSCoreValidationMessage("Broj telefona ne sme biti prazan")]
	SVC_005,

	[LSCoreValidationMessage("Broj telefona nije validan")]
	SVC_006,

	[LSCoreValidationMessage(
		"Ne mozete pokrenuti masovno slanje jer se oon trenutno nalazi u statusu {0}!"
	)]
	SVC_007,

	[LSCoreValidationMessage("Ne mozete pokrenuti maslovno slanje jer nema poruka za slanje!")]
	SVC_008
}
