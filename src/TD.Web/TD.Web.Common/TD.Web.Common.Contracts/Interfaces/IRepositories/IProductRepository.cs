using LSCore.Contracts.Interfaces.Repositories;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Interfaces.IRepositories;

public interface IProductRepository : ILSCoreRepositoryBase<ProductEntity>
{
    Task<Dictionary<long, ProductEntity>> GetAllAsDictionaryAsync();
}