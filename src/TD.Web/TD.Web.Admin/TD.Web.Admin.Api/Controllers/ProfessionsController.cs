using LSCore.Auth.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.Professions;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Professions;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers;

[LSCoreAuth]
[ApiController]
[Permissions(Permission.Access)]
public class ProfessionsController(IProfessionManager professionManager) : ControllerBase
{
	[HttpGet]
	[Route("/professions")]
	public List<ProfessionsGetMultipleDto> GetMultiple() => professionManager.GetMultiple();

	[HttpPut]
	[Route("/professions")]
	public long Save([FromBody] SaveProfessionRequest request) => professionManager.Save(request);
}
