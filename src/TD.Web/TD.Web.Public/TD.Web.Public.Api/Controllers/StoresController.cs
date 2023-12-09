using LSCore.Contracts.Http;
using LSCore.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Dtos.Stores;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Stores;

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
        public LSCoreSortedListResponse<StoreDto> GetMultiple([FromQuery] GetMultipleStoresRequest request) =>
            _storeManager.GetMultiple(request);
    }
}
