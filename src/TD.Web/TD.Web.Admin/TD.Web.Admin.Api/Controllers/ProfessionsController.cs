using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Professions;
using TD.Web.Admin.Contracts.Dtos.Professions;
using Microsoft.AspNetCore.Mvc;

namespace TD.Web.Admin.Api.Controllers;

[ApiController]
public class ProfessionsController (IProfessionManager professionManager) : ControllerBase
{
    [HttpGet]
    [Route("/professions")]
    public List<ProfessionsGetMultipleDto> GetMultiple() =>
        professionManager.GetMultiple();

    [HttpPut]
    [Route("/professions")]
    public long Save([FromBody] SaveProfessionRequest request) =>
        professionManager.Save(request);
}