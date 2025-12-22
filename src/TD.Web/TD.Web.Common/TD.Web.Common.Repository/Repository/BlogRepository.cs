using LSCore.Repository;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Common.Repository.Repository;

public class BlogRepository(WebDbContext dbContext)
	: LSCoreRepositoryBase<BlogEntity>(dbContext),
		IBlogRepository;
