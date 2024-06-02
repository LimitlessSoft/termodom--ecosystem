using LSCore.Contracts.Extensions;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Dtos.Stores;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Repository;
using LSCore.Domain.Managers;
using LSCore.Contracts.Http;

namespace TD.Office.Public.Domain.Managers
{
    public class StoreManager : LSCoreBaseManager<StoreManager>, IStoreManager
    {
        private readonly ITDKomercijalnoApiManager _tdKomercijalnoApiManager;
        
        public StoreManager(ILogger<StoreManager> logger, OfficeDbContext dbContext, ITDKomercijalnoApiManager tdKomercijalnoApiManager)
            : base(logger, dbContext)
        {
            _tdKomercijalnoApiManager = tdKomercijalnoApiManager;
        }

        public async Task<LSCoreListResponse<GetStoreDto>> GetMultiple()
        {
            var response = new LSCoreListResponse<GetStoreDto>();
            
            var magaciniResponse = await _tdKomercijalnoApiManager.GetMagacini();
            response.Merge(magaciniResponse);
            if (response.NotOk)
                return response;
            
            
            response.Payload = magaciniResponse.Payload!.Select(x => new GetStoreDto()
                {
                    Id = x.MagacinId,
                    Name = x.Naziv
                }).ToList();

            return response;
        }
    }
}