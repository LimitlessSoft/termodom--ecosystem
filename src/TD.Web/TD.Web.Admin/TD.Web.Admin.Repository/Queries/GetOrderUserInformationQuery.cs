using LSCore.Contracts.Http;
using LSCore.Contracts.Http.Interfaces;
using LSCore.Contracts.IManagers;
using LSCore.Repository;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Admin.Contracts.Requests.Orders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Repository.Queries
{
    public class GetOrderUserInformationQuery : LSCoreBaseQuery<OrderGetUserInformationRequest, OrderUserInformationDto>
    {
        public override ILSCoreResponse<OrderUserInformationDto> Execute(ILSCoreDbContext dbContext)
        {
            var response = new LSCoreResponse<OrderUserInformationDto>();

            var user = dbContext.AsQueryable<UserEntity>().FirstOrDefault(x => x.Id == Request!.UserId && x.IsActive);
            if (user == null)
                return LSCoreResponse<OrderUserInformationDto>.NotFound();

            response.Payload = new OrderUserInformationDto()
            {
                Id = user.Id,
                Mobile = user.Mobile,
                Name = user.Nickname
            };

            return response;
        }
    }
}
