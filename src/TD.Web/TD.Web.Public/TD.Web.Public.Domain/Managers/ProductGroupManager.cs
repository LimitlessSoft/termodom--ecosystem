using LSCore.Contracts.Dtos;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;
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

        public LSCoreListResponse<LSCoreIdNamePairDto> GetMultiple(ProductsGroupsGetRequest request)
        {
            var response = new LSCoreListResponse<LSCoreIdNamePairDto>();

            var groupIdResponse = First(x => request.ParentName != null && x.Name.Equals(request.ParentName));
            response.Merge(groupIdResponse);
            if (response.NotOk)
                return response;

            var groupId = groupIdResponse.Payload;

            var qResponse = Queryable(x =>
                x.IsActive &&
                (request.ParentId == null || x.ParentGroupId == request.ParentId) &&
                (groupId == null || x.ParentGroupId == groupId.Id));
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            response.Payload = qResponse.Payload!.ToDtoList<LSCoreIdNamePairDto, ProductGroupEntity>();
            return response;
        }
    }
}
