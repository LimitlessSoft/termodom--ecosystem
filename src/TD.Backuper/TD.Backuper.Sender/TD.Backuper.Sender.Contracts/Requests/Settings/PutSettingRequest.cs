using TD.Backuper.Sender.Contracts.Dtos.Settings;

namespace TD.Backuper.Sender.Contracts.Requests.Settings
{
    public class PutSettingRequest
    {
        public List<SettingPutRequestItemDto> SettingsItems { get; set; }
    }
}
