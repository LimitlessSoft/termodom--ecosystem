using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Stores;
using TD.Web.Common.Contracts.Dtos.Stores;
using Microsoft.AspNetCore.Mvc;

namespace TD.Web.Admin.Api.Controllers;

[ApiController]
public class StoresController (IStoreManager storeManager) : ControllerBase
{
    [HttpGet]
    [Route("/stores")]
    public List<StoreDto> GetMultiple([FromQuery] GetMultipleStoresRequest request) =>
        storeManager.GetMultiple(request);
}