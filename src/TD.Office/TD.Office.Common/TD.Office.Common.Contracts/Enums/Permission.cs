using System.ComponentModel;

namespace TD.Office.Common.Contracts.Enums
{
    public enum Permission
    {
        [Description("Pristup aplikaciji")]
        Access,
        [Description("Nalog za prevoz - pregled")]
        NalogZaPrevozRead,
    }
}