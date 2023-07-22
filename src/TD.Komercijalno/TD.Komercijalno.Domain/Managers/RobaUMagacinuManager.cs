using Microsoft.Extensions.Logging;
using TD.Core.Domain.Managers;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class RobaUMagacinuManager : BaseManager<RobaUMagacinuManager>, IRobaUMagacinuManager
    {
        public RobaUMagacinuManager(ILogger<RobaUMagacinuManager> logger, KomercijalnoDbContext dbContext)
            : base(logger, dbContext)
        {

        }
    }
}
