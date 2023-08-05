using TD.Backuper.Sender.Contracts.Dtos.Settings;
using TD.Backuper.Sender.Contracts.Requests.Settings;
using TD.Core.Contracts.Http;

namespace TD.Backuper.Sender.Contracts.IManagers
{
    public interface ISettingManager
    {
        ListResponse<SettingDto> GetMultiple();
        ListResponse<SettingDto> PutSetting(PutSettingRequest request);
    }
}
