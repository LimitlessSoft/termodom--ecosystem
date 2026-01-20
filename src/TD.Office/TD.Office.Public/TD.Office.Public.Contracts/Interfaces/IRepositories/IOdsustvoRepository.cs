using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface IOdsustvoRepository : ILSCoreRepositoryBase<OdsustvoEntity>
{
	List<OdsustvoEntity> GetByDateRange(DateTime startDate, DateTime endDate, int? storeId = null, long? userId = null);
}
