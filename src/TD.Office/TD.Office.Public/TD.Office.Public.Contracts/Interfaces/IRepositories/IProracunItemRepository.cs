using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface IProracunItemRepository : ILSCoreRepositoryBase<ProracunItemEntity>
{
	void UpdateKolicina(long id, decimal kolicina);
}
