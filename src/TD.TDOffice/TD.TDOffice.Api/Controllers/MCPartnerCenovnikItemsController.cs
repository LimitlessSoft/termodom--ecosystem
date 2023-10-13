using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikItems;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikItems;

namespace TD.TDOffice.Api.Controllers
{
    [ApiController]
    public class MCPartnerCenovnikItemsController : ControllerBase
    {
        private readonly IMCPartnerCenovnikItemManager _mcPartnerCenovnikItemManager;
        public MCPartnerCenovnikItemsController(IMCPartnerCenovnikItemManager mcPartnerCenovnikItemManager)
        {
            _mcPartnerCenovnikItemManager = mcPartnerCenovnikItemManager;
        }

        [HttpGet]
        [Route("/mc-partner-cenovnik-items")]
        public ListResponse<MCpartnerCenovnikItemEntityGetDto> GetMultiple([FromQuery] MCPartnerCenovnikItemGetRequest request)
        {
            return _mcPartnerCenovnikItemManager.GetMultiple(request);
        }

        [HttpPut]
        [Route("/mc-partner-cenovnik-items")]
        public Response<MCPartnerCenovnikItemEntity> Save([FromBody] SaveMCPartnerCenovnikItemRequest request)
        {
            return _mcPartnerCenovnikItemManager.Save(request);
        }

        [HttpDelete]
        [Route("/mc-partner-cenovnik-items/{Id}")]
        public Response Delete([FromRoute] IdRequest request)
        {
            return _mcPartnerCenovnikItemManager.Delete(request);
        }
    }
}
