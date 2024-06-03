using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Public.Contracts.Dtos.NalogZaPrevoz;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.NalogZaPrevoz;

namespace TD.Office.Public.Api.Controllers
{
    [ApiController]
    public class NalogZaPrevozController : ControllerBase
    {
        private readonly INalogZaPrevozManager _nalogZaPrevozManager;
        
        public NalogZaPrevozController(INalogZaPrevozManager nalogZaPrevozManager)
        {
            _nalogZaPrevozManager = nalogZaPrevozManager;
        }
        
        [HttpGet]
        [Route("/nalog-za-prevoz")]
        public LSCoreListResponse<GetNalogZaPrevozDto> GetMultiple([FromQuery] GetMultipleNalogZaPrevozRequest request) =>
            _nalogZaPrevozManager.GetMultiple(request);
        
        [HttpGet]
        [Route("/nalog-za-prevoz/{Id}")]
        public LSCoreResponse<GetNalogZaPrevozDto> GetSingle([FromRoute] LSCoreIdRequest request) =>
            _nalogZaPrevozManager.GetSingle(request);
        
        [HttpPut]
        [Route("/nalog-za-prevoz")]
        public LSCoreResponse SaveNalogZaPrevoz([FromBody] SaveNalogZaPrevozRequest request) =>
            _nalogZaPrevozManager.SaveNalogZaPrevoz(request);
        
        [HttpGet]
        [Route("/nalog-za-prevoz-referentni-dokument")]
        public async Task<LSCoreResponse<GetReferentniDokumentNalogZaPrevozDto>> GetReferentniDokument([FromQuery] GetReferentniDokumentNalogZaPrevozRequest request) =>
            await _nalogZaPrevozManager.GetReferentniDokument(request);
    }
}