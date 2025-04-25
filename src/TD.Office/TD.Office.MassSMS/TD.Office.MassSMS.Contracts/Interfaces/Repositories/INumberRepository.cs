using LSCore.Repository.Contracts;
using TD.Office.MassSMS.Contracts.Entities;

namespace TD.Office.MassSMS.Contracts.Interfaces.Repositories;

public interface INumberRepository : ILSCoreRepositoryBase<NumberEntity>
{
	NumberEntity? GetOrDefault(string number);
	NumberEntity Get(string number);
	void Insert(string number);
	Task InsertAsync(params string[] number);
	List<NumberEntity> GetBlacklisted();
	void InsertBlacklisted(string number);
	void RemoveBlacklisted(string number);
	bool IsBlacklisted(string number);
}
