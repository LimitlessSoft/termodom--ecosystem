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
            var user = FirstOrDefault(x => x.Id == request.Id);

            if (user == null)
                return Response<UserDto>.NotFound();

            return new Response<UserDto>(user.ToUserDto());
        }

        public ListResponse<UserDto> GetMultiple()
        {
            return new ListResponse<UserDto>(Queryable().ToList().ToUserDtoList());
        }
    }
}
