using LSCore.Exceptions;
using LSCore.Repository;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class KomercijalnoIFinansijskoPoGodinamaRepository(OfficeDbContext dbContext)
	: LSCoreRepositoryBase<KomercijalnoIFinansijskoPoGodinamaEntity>(dbContext),
		IKomercijalnoIFinansijskoPoGodinamaRepository
{
	public KomercijalnoIFinansijskoPoGodinamaEntity GetByPPID(int PPID)
	{
		var entities = dbContext.KomercijalnoIFinansijskoPoGodinama.FirstOrDefault(x =>
			x.IsActive && x.PPID == PPID
		);
		if (entities == null)
			throw new LSCoreNotFoundException();
		return entities;
	}

	public KomercijalnoIFinansijskoPoGodinamaEntity Create(int ppid, long statusDefaultId)
	{
		var entity = new KomercijalnoIFinansijskoPoGodinamaEntity
		{
			PPID = ppid,
			StatusId = statusDefaultId
		};
		Insert(entity);
		return entity;
	}
}
