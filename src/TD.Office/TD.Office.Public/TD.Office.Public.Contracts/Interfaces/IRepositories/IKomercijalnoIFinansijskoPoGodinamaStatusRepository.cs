using LSCore.Contracts.Interfaces.Repositories;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories
{
    public interface IKomercijalnoIFinansijskoPoGodinamaStatusRepository : ILSCoreRepositoryBase<KomercijalnoIFinansijskoPoGodinamaStatusEntity>
    {
        /// <summary>
        /// Returns Id of the entity which is marked default and IsActive is true
        /// </summary>
        /// <returns></returns>
        long GetDefaultId();
        List<KomercijalnoIFinansijskoPoGodinamaStatusEntity> GetAllStatuses();
    }
}
