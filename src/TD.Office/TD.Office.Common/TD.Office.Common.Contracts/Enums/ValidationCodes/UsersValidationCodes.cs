using LSCore.Validation.Contracts;

namespace TD.Office.Common.Contracts.Enums.ValidationCodes;

public enum UsersValidationCodes
{
	[LSCoreValidationMessage("Korisničko ime ili lozinka nisu ispravni!.")]
	UVC_001
}
