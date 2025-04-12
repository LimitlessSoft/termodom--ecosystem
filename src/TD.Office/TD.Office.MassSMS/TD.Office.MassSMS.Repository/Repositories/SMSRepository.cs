using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using TD.Office.MassSMS.Contracts.Entities;
using TD.Office.MassSMS.Contracts.Interfaces;
using TD.Office.MassSMS.Contracts.Interfaces.Repositories;

namespace TD.Office.MassSMS.Repository.Repositories;

public class SMSRepository(IMassSMSContext dbContext)
	: LSCoreRepositoryBase<SMSEntity>(dbContext),
		ISMSRepository
{
	public void ClearDuplicates()
	{
		var duplicates = GetMultiple()
			.ToList()
			.GroupBy(sms => sms.Phone)
			.Where(group => group.Count() > 1)
			.ToList();
		var idsOfDuplicates = duplicates
			.SelectMany(group => group.Skip(1).Select(sms => sms.Id))
			.ToList();
		HardDelete(idsOfDuplicates);
	}

	public void SetText(string text) =>
		dbContext.SMSs.ExecuteUpdate(x => x.SetProperty(sms => sms.Text, text));
}
