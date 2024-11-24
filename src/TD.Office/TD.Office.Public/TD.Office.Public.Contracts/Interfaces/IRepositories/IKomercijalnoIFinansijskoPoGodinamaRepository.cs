using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;
public interface IKomercijalnoIFinansijskoPoGodinamaRepository
{
    KomercijalnoIFinansijskoPoGodinamaEntity GetByPPID(int PPID);
}
