using LSCore.Repository.Contracts;
using TD.Office.MassSMS.Contracts.Entities;

namespace TD.Office.MassSMS.Contracts.Interfaces.Repositories;

public interface ISMSRepository : ILSCoreRepositoryBase<SMSEntity>
{
	void ClearDuplicates();
	void SetText(string text);
}
