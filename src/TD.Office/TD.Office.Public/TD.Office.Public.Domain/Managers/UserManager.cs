using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using LSCore.Domain.Validators;
using LSCore.Domain.Extensions;
using LSCore.Contracts.Extensions;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Repository;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Domain.Extensions;
using Microsoft.Extensions.Configuration;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Requests.Users;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Dtos.Users;

namespace TD.Office.Public.Domain.Managers
{
    public class UserManager : LSCoreBaseManager<UserManager, UserEntity>, IUserManager
    {
        private readonly IConfigurationRoot _configurationRoot;

        public UserManager(ILogger<UserManager> logger, OfficeDbContext dbContext, IConfigurationRoot configurationRoot)
            : base(logger, dbContext)
        {
            _configurationRoot = configurationRoot;
        }

        public LSCoreResponse<string> Login(UsersLoginRequest request)
        {
            var response = new LSCoreResponse<string>();

            if(request.IsRequestInvalid(response))
                return response;

            var qUserResponse = Queryable()
                .LSCoreFilters(x => x.IsActive && x.Username.ToUpper() == request.Username.ToUpper());
            response.Merge(qUserResponse);
            if (response.NotOk)
                return response;

            var user = qUserResponse.Payload!
                .AsNoTrackingWithIdentityResolution()
                .First();

            return new LSCoreResponse<string>(user.GenerateJSONWebToken(_configurationRoot));
        }

        public LSCoreResponse<UserMeDto> Me() =>
            new LSCoreResponse<UserMeDto>(First(x => CurrentUser != null && x.Id == CurrentUser.Id && x.IsActive).Payload!.ToDto<UserMeDto, UserEntity>());
    }
}
