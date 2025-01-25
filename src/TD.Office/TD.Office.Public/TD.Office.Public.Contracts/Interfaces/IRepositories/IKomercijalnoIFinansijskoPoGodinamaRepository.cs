using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface IKomercijalnoIFinansijskoPoGodinamaRepository
{
    KomercijalnoIFinansijskoPoGodinamaEntity Get(long id);
    KomercijalnoIFinansijskoPoGodinamaEntity? GetOrDefault(long id);
    KomercijalnoIFinansijskoPoGodinamaEntity GetByPPID(int PPID);
    KomercijalnoIFinansijskoPoGodinamaEntity Create(int ppid, long statusDefaultId);
    void Update(KomercijalnoIFinansijskoPoGodinamaEntity entity);
}
