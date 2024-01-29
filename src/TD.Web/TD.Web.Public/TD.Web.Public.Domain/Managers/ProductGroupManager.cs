using LSCore.Contracts.Dtos;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Dtos.ProductsGroups;
using TD.Web.Public.Contrats.Interfaces.IManagers;
using TD.Web.Public.Contrats.Requests.ProductsGroups;

namespace TD.Web.Public.Domain.Managers
{
    public class ProductGroupManager : LSCoreBaseManager<ProductGroupManager, ProductGroupEntity>, IProductGroupManager
    {
        public ProductGroupManager(ILogger<ProductGroupManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreResponse<ProductsGroupsGetDto> Get(string name)
        {
            var response = new LSCoreResponse<ProductsGroupsGetDto>();

            var qResponse = Queryable();
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            response.Payload = qResponse.Payload!
                .Include(x => x.ParentGroup)
                .Where(x =>
                    x.IsActive &&
                    x.Name == name)
                .FirstOrDefault()?
                .ToDto<ProductsGroupsGetDto, ProductGroupEntity>(); ;

            return response;
        }

        public LSCoreListResponse<ProductsGroupsGetDto> GetMultiple(ProductsGroupsGetRequest request)
        {
            var response = new LSCoreListResponse<ProductsGroupsGetDto>();

            var qResponse = Queryable(x => x.IsActive);
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            response.Payload = qResponse.Payload!
                .Include(x => x.ParentGroup)
                .Where(x =>
                    (!request.ParentId.HasValue && !string.IsNullOrWhiteSpace(request.ParentName) || x.ParentGroupId == request.ParentId) &&
                    (string.IsNullOrWhiteSpace(request.ParentName) || x.ParentGroup != null && x.ParentGroup.Name == request.ParentName))
                .ToDtoList<ProductsGroupsGetDto, ProductGroupEntity>();
            return response;
        }
    }
}
