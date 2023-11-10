using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class RobaUMagacinuManager : LSCoreBaseManager<RobaUMagacinuManager>, IRobaUMagacinuManager
    {
        public RobaUMagacinuManager(ILogger<RobaUMagacinuManager> logger, KomercijalnoDbContext dbContext)
            : base(logger, dbContext)
        {

        }
    }
}
