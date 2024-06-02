using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Public.Contracts.Dtos.Stores;
using TD.Office.Public.Contracts.Interfaces.IManagers;

namespace TD.Office.Public.Api
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
        public async Task<LSCoreListResponse<GetStoreDto>> GetMultiple() => await _storeManager.GetMultiple();
    }
}