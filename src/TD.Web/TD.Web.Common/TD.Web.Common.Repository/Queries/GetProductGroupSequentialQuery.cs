using LSCore.Contracts.Http;
using LSCore.Contracts.Http.Interfaces;
using LSCore.Contracts.IManagers;
using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Dtos.ProductsGroups;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Requests.ProductsGroups;

namespace TD.Web.Common.Repository.Queries
{
    public class GetProductGroupSequentialQuery : LSCoreBaseQuery<GetParentGroupSequentialRequest, List<GetProductGroupSequentialDto>>
    {
        public override ILSCoreResponse<List<GetProductGroupSequentialDto>> Execute(ILSCoreDbContext dbContext)
        {
            var response = new LSCoreListResponse<GetProductGroupSequentialDto>();
            var productGroupSquential = string.Empty;
            var parentGroup = dbContext.AsQueryable<ProductEntity>()
                .Include(x => x.Groups)
                .ThenInclude(x => x.ParentGroup)
                .FirstOrDefault(x => x.Id == Request!.ProductId && x.IsActive);

            if(parentGroup != null)
                foreach (var group in parentGroup.Groups)
                    response.Payload.Add(buildTree(group, dbContext));

            return response;
        }

        private GetProductGroupSequentialDto buildTree(ProductGroupEntity? group, ILSCoreDbContext dbContext)
        {
            var response = new GetProductGroupSequentialDto();
            response.Name = group.Name;
            group = dbContext.AsQueryable<ProductGroupEntity>()
                .Include(x => x.ParentGroup)
                .FirstOrDefault(x => group.ParentGroupId == x.Id && x.IsActive);

            while (group != null)
            {
                var oldResponse = response;
                response = new GetProductGroupSequentialDto();
                response.Child = oldResponse;
                response.Name = group.Name;

                group = dbContext.AsQueryable<ProductGroupEntity>()
                .Include(x => x.ParentGroup)
                .FirstOrDefault(x => group.ParentGroupId == x.Id && x.IsActive);
            }

            return response;
        }
    }
}
