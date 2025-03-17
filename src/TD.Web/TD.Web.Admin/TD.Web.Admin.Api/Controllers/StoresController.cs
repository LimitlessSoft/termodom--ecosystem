using LSCore.Auth.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Dtos.Stores;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Stores;

namespace TD.Web.Admin.Api.Controllers;

[LSCoreAuth]
[ApiController]
[Permissions(Permission.Access)]
public class StoresController(IStoreManager storeManager) : ControllerBase
{
	[HttpGet]
	[Route("/stores")]
	public List<StoreDto> GetMultiple([FromQuery] GetMultipleStoresRequest request) =>
		storeManager.GetMultiple(request);
}
