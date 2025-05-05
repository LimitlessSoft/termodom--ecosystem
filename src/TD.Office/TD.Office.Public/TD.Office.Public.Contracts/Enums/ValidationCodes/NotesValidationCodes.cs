using System.ComponentModel;

namespace TD.Office.Public.Contracts.Enums.ValidationCodes;

public enum NotesValidationCodes
{
	[Description("Tab sa istim imenom već postoji.")]
	NVC_001,

	[Description("Brisanje nije dozvoljeno. Morate imati najmanje jednu belešku.")]
	NVC_002,

	[Description(
		"Došlo je do izmene u međuvremenu. Osvežite stranicu. Kopirajte vaš tekst pre nego što nastavite."
	)]
	NVC_003
}
