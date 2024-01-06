using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Dtos.Stores;
using TD.Web.Common.Contracts.Requests.Stores;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Public.Api.Controllers
{
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreManager _storeManager;

        public StoresController(IStoreManager storeManager)
        {
            _storeManager = storeManager;
        }

        [HttpGet]
        [Route("/stores")]
        public LSCoreListResponse<StoreDto> GetMultiple([FromQuery] GetMultipleStoresRequest request) =>
            _storeManager.GetMultiple(request);
    }
}
