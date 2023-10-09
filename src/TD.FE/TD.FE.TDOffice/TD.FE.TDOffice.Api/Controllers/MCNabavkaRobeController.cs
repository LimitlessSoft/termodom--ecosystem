using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.MCNabavkaRobe;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.FE.TDOffice.Contracts.Requests.MCNabavkaRobe;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikKatBrRobaId;

namespace TD.FE.TDOffice.Api.Controllers
{
    [ApiController]
    public class MCNabavkaRobeController : ControllerBase
    {
        private readonly IMCNabavkaRobeManager _mcNabavkaRobeManager;
        private readonly ITDOfficeApiManager _tdOfficeApiManager;
        public MCNabavkaRobeController(IMCNabavkaRobeManager mcNabavkaRobeManager, ITDOfficeApiManager tdOfficeApiManager)
        {
            _mcNabavkaRobeManager = mcNabavkaRobeManager;
            _tdOfficeApiManager = tdOfficeApiManager;
        }

        [HttpPost]
        [Route("/mc-nabavka-robe-uvuci-fajl")]
        public async Task<ListResponse<CenovnikItem>> UvuciFajl([FromForm] MCNabavkaRobeUvuciFajlRequest request)
        {
            return await _mcNabavkaRobeManager.UvuciFajlAsync(request);
        }

        [HttpPut]
        [Route("/mc-nabavka-robe-sacuvaj-partner-cenovnik-item")]
        public async Task<Response<MCPartnerCenovnikKatBrRobaIdEntity>> SacuvajPartnerCenovnikItem(MCPartnerCenovnikKatBrRobaIdSaveRequest request)
        {
            return await _tdOfficeApiManager.PutAsync<MCPartnerCenovnikKatBrRobaIdSaveRequest, MCPartnerCenovnikKatBrRobaIdEntity>("/mc-partner-cenovnik-kat-br-roba-ids", request);
        }
    }
}
