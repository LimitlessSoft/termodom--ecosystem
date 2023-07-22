using System.ComponentModel;

namespace TD.TDOffice.Contracts.Enums.ValidationCodes
{
    public static partial class ValidationCodes
    {
        public enum DokumentTagIzvodValidationCodes
        {
            [Description("Ne mozete azurirati broj dokumenta izvoda na postojecem recordu!")]
            DTI001,
            [Description("Nesto Drugo")]
            DTI002
        }
    }
}
