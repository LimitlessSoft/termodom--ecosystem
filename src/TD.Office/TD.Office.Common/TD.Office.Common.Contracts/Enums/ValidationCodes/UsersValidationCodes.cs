using System.ComponentModel;

namespace TD.Office.Common.Contracts.Enums.ValidationCodes;

public enum UsersValidationCodes
{
    [Description("Korisničko ime ili lozinka nisu ispravni!.")]
    UVC_001,
    [Description("Korisnik sa datim identifikatorom ne postoji.")]
    UVC_002
}