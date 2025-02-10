using LSCore.Contracts.IManagers;
using LSCore.Repository;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Common.Repository.Repository;

public class KomercijalnoWebProductLinkRepository (WebDbContext dbContext)
    : LSCoreRepositoryBase<KomercijalnoWebProductLinkEntity>(dbContext), IKomercijalnoWebProductLinkRepository;