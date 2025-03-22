using LSCore.Repository;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Common.Repository.Repository;

public class ProductGroupRepository(WebDbContext dbContext)
	: LSCoreRepositoryBase<ProductGroupEntity>(dbContext),
		IProductGroupRepository;
