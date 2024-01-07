using System.ComponentModel;

namespace TD.Web.Common.Contracts.Enums.ValidationCodes
{
    public enum OrderItemsValidationCodes
    {
        [Description("Proizvod ne postoji.")]
        OIVC_001,
        [Description("Proizvod nije u korpi.")]
        OIVC_002,
    }
}
