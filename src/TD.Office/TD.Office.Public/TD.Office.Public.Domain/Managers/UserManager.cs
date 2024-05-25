using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using LSCore.Domain.Validators;
using LSCore.Domain.Extensions;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Repository;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Domain.Extensions;
using Microsoft.Extensions.Configuration;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.Requests.Users;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Dtos.Users;
using TD.Office.Public.Contracts.Enums.SortColumnCodes;
using TD.Office.Public.Contracts.Requests.Users;

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
            new (First(x => CurrentUser != null && x.Id == CurrentUser.Id && x.IsActive).Payload!.ToDto<UserMeDto, UserEntity>());

        public LSCoreResponse<UserDto> GetSingle(LSCoreIdRequest request) =>
            new (First(x => x.Id == request.Id && x.IsActive).Payload!.ToDto<UserDto, UserEntity>());
        
        public LSCoreSortedPagedResponse<UserDto> GetMultiple(UsersGetMultipleRequest request)
        {
            var response = new LSCoreSortedPagedResponse<UserDto>();
            
            var qUserResponse = Queryable()
                .LSCoreFilters(x => x.IsActive);
            
            response.Merge(qUserResponse);
            return response.NotOk
                ? response
                : qUserResponse.ToLSCoreSortedPagedResponse<UserDto, UserEntity, UsersSortColumnCodes.Users>(request, UsersSortColumnCodes.UsersSortRules);
        }

        public LSCoreResponse UpdateNickname(UsersUpdateNicknameRequest request)
        {
            var response = new LSCoreResponse();
            
            var saveResponse = Save(request);
            response.Merge(saveResponse);
            
            return response;
        }

        public LSCoreResponse<UserDto> Create(UsersCreateRequest request)
        {
            var response = new LSCoreResponse<UserDto>();
            
            if (request.IsRequestInvalid(response))
                return response;

            response.Merge(Insert(new UserEntity()
            {
                IsActive = true,
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password),
                Nickname = request.Nickname,
                Type = UserType.User
            }));
            
            return response;
        }
    }
}
