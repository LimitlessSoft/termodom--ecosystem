using LSCore.Repository;
using TD.Office.MassSMS.Contracts.Entities;
using TD.Office.MassSMS.Contracts.Interfaces;
using TD.Office.MassSMS.Contracts.Interfaces.Repositories;

namespace TD.Office.MassSMS.Repository.Repositories;

public class SMSRepository(IMassSMSContext dbContext)
	: LSCoreRepositoryBase<SMSEntity>(dbContext),
		ISMSRepository;
