using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikItems;
using TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikItems;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Contracts.Entities;
using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace TD.TDOffice.Api.Controllers;

[ApiController]
public class MCPartnerCenovnikItemsController (IMCPartnerCenovnikItemManager mcPartnerCenovnikItemManager)
    : ControllerBase
{
    [HttpGet]
    [Route("/mc-partner-cenovnik-items")]
    public List<MCpartnerCenovnikItemEntityGetDto> GetMultiple([FromQuery] MCPartnerCenovnikItemGetRequest request) =>
        mcPartnerCenovnikItemManager.GetMultiple(request);

    [HttpPut]
    [Route("/mc-partner-cenovnik-items")]
    public MCPartnerCenovnikItemEntity Save([FromBody] SaveMCPartnerCenovnikItemRequest request) =>
        mcPartnerCenovnikItemManager.Save(request);

    [HttpDelete]
    [Route("/mc-partner-cenovnik-items/{Id}")]
    public void Delete([FromRoute] LSCoreIdRequest request) =>
        mcPartnerCenovnikItemManager.Delete(request);
}