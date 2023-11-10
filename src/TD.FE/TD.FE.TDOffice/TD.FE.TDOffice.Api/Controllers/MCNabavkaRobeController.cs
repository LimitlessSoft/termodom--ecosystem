using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Proverava da li partner ima sacuvan cenovnik u bazi za dati dan.
        /// Ukoliko ima vraca true
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/mc-nabavka-robe-proveri-postojanje-cenovnika-na-dan")]
        public async Task<LSCoreResponse<bool>> ProveriPostojanjeCenovnikaNaDan([FromQuery] MCNabavkaRobeProveriPostojanjeCenovnikaNaDanRequest request)
        {
            return await _mcNabavkaRobeManager.ProveriPostojanjeCenovnikaNaDan(request);
        }

        [HttpGet]
        [Route("/mc-nabavka-robe-uporedi-cenovnike")]
        public async Task<LSCoreListResponse<MCNabavkaRobeUporediCenovnikeItemDto>> UporediCenovnike()
        {
            return await _mcNabavkaRobeManager.UporediCenovnikeAsync();
        }

        [HttpPost]
        [Route("/mc-nabavka-robe-uvuci-fajl")]
        public async Task<LSCoreListResponse<CenovnikItem>> UvuciFajl([FromForm] MCNabavkaRobeUvuciFajlRequest request)
        {
            return await _mcNabavkaRobeManager.UvuciFajlAsync(request);
        }

        [HttpPut]
        [Route("/mc-nabavka-robe-sacuvaj-partner-cenovnik-item")]
        public async Task<LSCoreResponse<MCPartnerCenovnikKatBrRobaIdEntity>> SacuvajPartnerCenovnikItem(MCPartnerCenovnikKatBrRobaIdSaveRequest request)
        {
            return await _tdOfficeApiManager.PutAsync<MCPartnerCenovnikKatBrRobaIdSaveRequest, MCPartnerCenovnikKatBrRobaIdEntity>("/mc-partner-cenovnik-kat-br-roba-ids", request);
        }
    }
}
