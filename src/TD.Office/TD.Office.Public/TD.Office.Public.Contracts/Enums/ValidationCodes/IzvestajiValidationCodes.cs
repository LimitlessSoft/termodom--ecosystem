using System.ComponentModel;

namespace TD.Office.Public.Contracts.Enums.ValidationCodes;

public enum IzvestajiValidationCodes
{
    [Description(
        "Magacin {0} nije svrstan ni u jedan centar. Svrstajte magacin u centar i pokrenite izvestaj ponovo."
    )]
    IVC_001,

    [Description("Godina mora biti veća ili jednaka {0}.")]
    IVC_002
}
