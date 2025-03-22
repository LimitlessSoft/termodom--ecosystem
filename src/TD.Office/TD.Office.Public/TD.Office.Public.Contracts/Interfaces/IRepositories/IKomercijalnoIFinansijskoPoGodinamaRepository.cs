using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface IKomercijalnoIFinansijskoPoGodinamaRepository
	: ILSCoreRepositoryBase<KomercijalnoIFinansijskoPoGodinamaEntity>
{
	KomercijalnoIFinansijskoPoGodinamaEntity Create(int ppid, long statusDefaultId);
	KomercijalnoIFinansijskoPoGodinamaEntity GetByPPID(int PPID);
}
