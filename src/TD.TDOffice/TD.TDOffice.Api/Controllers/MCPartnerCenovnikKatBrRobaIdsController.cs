using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikKatBrRobaIds;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.Helpers.MCPartnerCenovnikKatBrRobaIdsHelpers;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikKatBrRobaId;

namespace TD.TDOffice.Api.Controllers
{
    public class MCPartnerCenovnikKatBrRobaIdsController : ControllerBase
    {
        private readonly IMCPartnerCenovnikKatBrRobaIdManager _mcPartnerCenovnikKatBrRobaIdManager;
        public MCPartnerCenovnikKatBrRobaIdsController(IMCPartnerCenovnikKatBrRobaIdManager mcPartnerCenovnikKatBrRobaIdManager)
        {
            _mcPartnerCenovnikKatBrRobaIdManager = mcPartnerCenovnikKatBrRobaIdManager;
        }

        [HttpGet]
        [Route("/mc-partner-cenovnik-kat-br-roba-ids")]
        public ListResponse<MCPartnerCenovnikKatBrRobaIdGetDto> GetMultiple([FromQuery] MCPartnerCenovnikKatBrRobaIdsGetMultipleRequest request)
        {
            var response = new ListResponse<MCPartnerCenovnikKatBrRobaIdGetDto>();

            var pcResponse = _mcPartnerCenovnikKatBrRobaIdManager.GetMultiple(request);
            response.Merge(pcResponse);
            if (response.NotOk)
                return response;

            response.Payload = pcResponse.Payload.ToListDto();
            return response;
        }

        [HttpPut]
        [Route("/mc-partner-cenovnik-kat-br-roba-ids")]
        public Response<MCPartnerCenovnikKatBrRobaIdEntity> Save([FromBody] MCPartnerCenovnikKatBrRobaIdSaveRequest request)
        {
            return _mcPartnerCenovnikKatBrRobaIdManager.Save(request);
        }
    }
}
