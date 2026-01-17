using LSCore.Repository;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class TipOdsustvaRepository(OfficeDbContext dbContext)
	: LSCoreRepositoryBase<TipOdsustvaEntity>(dbContext),
		ITipOdsustvaRepository
{
}
