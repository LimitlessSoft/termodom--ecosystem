using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Public.Contracts.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Dictionary<long, ProductEntity>> GetAllAsDictionaryAsync();
}
