using Api.Dtos;
using Infrastructure.Entities.ApiV2;
using Infrastructure.Framework;

namespace Api.Interfaces.Managers
{
    public interface IProductsManager
    {
        public APIResponse<IQueryable<PublicProductDto>> Queriable();
    }
}
