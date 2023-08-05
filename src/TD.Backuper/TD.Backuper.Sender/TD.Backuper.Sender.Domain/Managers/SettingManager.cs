using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TD.Backuper.Sender.Contracts;
using TD.Backuper.Sender.Contracts.Dtos.Settings;
using TD.Backuper.Sender.Contracts.IManagers;
using TD.Backuper.Sender.Contracts.Requests.Settings;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;

namespace TD.Backuper.Sender.Domain.Managers
{
    public class SettingManager : BaseManager<SettingManager>, ISettingManager
    {
        public SettingManager(ILogger<SettingManager> logger)
            : base(logger)
        {
            if (!File.Exists(Constants.BackupSettingsFilePath))
                File.WriteAllText(Constants.BackupSettingsFilePath, JsonConvert.SerializeObject(new List<SettingDto>()));
        }

        public ListResponse<SettingDto> GetMultiple()
        {
            var obj = JsonConvert.DeserializeObject<List<SettingDto>>(
                File.ReadAllText(Constants.BackupSettingsFilePath));

            if (obj == null)
                return ListResponse<SettingDto>.NotFound();

            return new ListResponse<SettingDto>(obj);
        }

        public ListResponse<SettingDto> PutSetting(PutSettingRequest request)
        {
            var response = new ListResponse<SettingDto>();
            if (request.IsRequestInvalid(response))
                return response;

            var existingSettings = JsonConvert.DeserializeObject<List<SettingDto>>(File.ReadAllText(Constants.BackupSettingsFilePath)) ?? new List<SettingDto>();

            var bufferList = new List<SettingDto>();
            foreach(var item in request.SettingsItems)
            {
                bufferList.Add(new SettingDto()
                {
                    Path = item.Path,
                    IntervalMinutes = item.IntervalMinutes
                });
            }

            foreach(var item in existingSettings)
            {
                var itemFromBufferList = bufferList.FirstOrDefault(x => x.Path.ToLower() == item.Path.ToLower());

                if (itemFromBufferList == null)
                    continue;

                itemFromBufferList.LastTimeBackup = item.LastTimeBackup;
            }

            File.WriteAllText(Constants.BackupSettingsFilePath, JsonConvert.SerializeObject(bufferList));

            return new ListResponse<SettingDto>(bufferList);
        }
    }
}
