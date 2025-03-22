using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface IKomercijalnoPriceRepository : ILSCoreRepositoryBase<KomercijalnoPriceEntity>
{
	void HardClear();
}
