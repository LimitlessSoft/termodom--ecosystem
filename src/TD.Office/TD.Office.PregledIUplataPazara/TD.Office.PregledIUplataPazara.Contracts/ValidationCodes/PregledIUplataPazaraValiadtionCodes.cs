using LSCore.Validation.Contracts;

namespace TD.Office.PregledIUplataPazara.Contracts.ValidationCodes;

public enum PregledIUplataPazaraValiadtionCodes
{
	[LSCoreValidationMessage("'Od Datuma' je obavezan.")]
	PUPVC_001,

	[LSCoreValidationMessage("'Do Datuma' je obavezan.")]
	PUPVC_002,

	[LSCoreValidationMessage("Obavezno je selektovati barem jedan magacin.")]
	PUPVC_003,

	[LSCoreValidationMessage("'Do datuma' ne moze biti manji od 'Od datuma'.")]
	PUPVC_004,

	[LSCoreValidationMessage("Tolerancija je obavezna.")]
	PUPVC_005
}
