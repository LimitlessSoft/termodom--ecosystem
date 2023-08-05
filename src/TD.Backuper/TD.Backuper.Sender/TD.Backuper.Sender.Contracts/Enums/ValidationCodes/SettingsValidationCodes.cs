using System.ComponentModel;

namespace TD.Backuper.Sender.Contracts.Enums.ValidationCodes
{
    public enum SettingsValidationCodes
    {
        [Description("Setting parametar '{0}' mora imati vrednost!")]
        SV001,
        [Description("Setting parametar '{0}' mora biti veci od 0!")]
        SV002
    }
}
