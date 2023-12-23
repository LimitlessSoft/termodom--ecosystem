using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.TDOffice.Contracts.DtoMappings.Users;
using TD.TDOffice.Contracts.Dtos.Users;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Repository;

namespace TD.TDOffice.Domain.Managers
{
    public class UserManager : LSCoreBaseManager<UserManager, User>, IUserManager
    {
        public UserManager(ILogger<UserManager> logger, TDOfficeDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreResponse<UserDto> Get(LSCoreIdRequest request)
        {
            var response = new LSCoreResponse<UserDto>();
            var userResponse = First(x => x.Id == request.Id);

            if(userResponse.Status == System.Net.HttpStatusCode.NotFound)
                return LSCoreResponse<UserDto>.NotFound();

            response.Merge(userResponse);
            if (response.NotOk)
                return response;

            response.Payload = userResponse.Payload.ToUserDto();
            return response;
        }

        public LSCoreListResponse<UserDto> GetMultiple()
        {
            var response = new LSCoreListResponse<UserDto>();

            var qResponse = Queryable(x => x.IsActive);
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            response.Payload = qResponse.Payload!.ToList().ToUserDtoList();
            return response;
        }
    }
}
