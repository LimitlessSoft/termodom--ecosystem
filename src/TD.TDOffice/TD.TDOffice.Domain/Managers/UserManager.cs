using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Managers;
using TD.TDOffice.Contracts.DtoMappings.Users;
using TD.TDOffice.Contracts.Dtos.Users;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Repository;

namespace TD.TDOffice.Domain.Managers
{
    public class UserManager : BaseManager<UserManager, User>, IUserManager
    {
        public UserManager(ILogger<UserManager> logger, TDOfficeDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public Response<UserDto> Get(IdRequest request)
        {
            var response = new Response<UserDto>();
            var userResponse = First(x => x.Id == request.Id);

            if(userResponse.Status == System.Net.HttpStatusCode.NotFound)
                return Response<UserDto>.NotFound();

            response.Merge(userResponse);
            if (response.NotOk)
                return response;

            response.Payload = userResponse.Payload.ToUserDto();
            return response;
        }

        public ListResponse<UserDto> GetMultiple()
        {
            return new ListResponse<UserDto>(Queryable().ToList().ToUserDtoList());
        }
    }
}
