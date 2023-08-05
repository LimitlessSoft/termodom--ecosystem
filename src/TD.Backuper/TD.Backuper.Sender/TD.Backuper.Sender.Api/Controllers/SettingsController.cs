using Microsoft.AspNetCore.Mvc;
using TD.Backuper.Sender.Contracts.Dtos.Settings;
using TD.Backuper.Sender.Contracts.IManagers;
using TD.Backuper.Sender.Contracts.Requests.Settings;
using TD.Core.Contracts.Http;

namespace TD.Backuper.Sender.Api.Controllers
{
    [ApiController]
    public class SettingsController : Controller
    {
        private readonly ISettingManager _settingManager;
        
        public SettingsController(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        [HttpGet]
        [Route("/settings")]
        public ListResponse<SettingDto> GetMultiple()
        {
            return _settingManager.GetMultiple();
        }

        [HttpPut]
        [Route("/settings")]
        public ListResponse<SettingDto> PutSetting([FromBody] PutSettingRequest request)
        {
            return _settingManager.PutSetting(request);
        }
    }
}
