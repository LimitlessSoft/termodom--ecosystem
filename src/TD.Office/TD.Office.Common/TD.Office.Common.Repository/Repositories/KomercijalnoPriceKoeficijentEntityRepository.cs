using LSCore.Repository;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.IRepositories;

namespace TD.Office.Common.Repository.Repositories;

public class KomercijalnoPriceKoeficijentEntityRepository(OfficeDbContext dbContext)
	: LSCoreRepositoryBase<KomercijalnoPriceKoeficijentEntity>(dbContext),
		IKomercijalnoPriceKoeficijentEntityRepository { }
