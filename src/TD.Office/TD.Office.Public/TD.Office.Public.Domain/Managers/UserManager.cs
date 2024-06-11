using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Users;
using TD.Office.Common.Contracts.Requests.Users;
using TD.Office.Public.Contracts.Dtos.Users;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Domain.Extensions;
using Microsoft.Extensions.Configuration;
using TD.Office.Common.Contracts.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Exceptions;
using TD.Office.Common.Repository;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;

namespace TD.Office.Public.Domain.Managers
{
    public class UserManager (
        ILogger<UserManager> logger,
        OfficeDbContext dbContext,
        IConfigurationRoot configurationRoot)
        : LSCoreManagerBase<UserManager, UserEntity>(logger, dbContext), IUserManager
    {
        public string Login(UsersLoginRequest request)
        {
            request.Validate();

            var user = Queryable()
                .Include(x => x.Permissions)
                .Where(x =>
                    x.IsActive
                    && x.Username.ToUpper() == request.Username!.ToUpper()
                    && x.Permissions!.Count > 0
                    && x.Permissions!.Any(z =>
                        z.IsActive
                        && z.Permission == Permission.Access))
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault();

            if (user == null)
                throw new LSCoreForbiddenException();
            
            return user.GenerateJSONWebToken(configurationRoot);
        }

        public UserMeDto Me() =>
            Queryable().FirstOrDefault(x => x.IsActive && x.Id == CurrentUser!.Id)?.ToDto<UserEntity, UserMeDto>()
            ?? new UserMeDto();

        public UserDto GetSingle(LSCoreIdRequest request) =>
            Queryable().FirstOrDefault(x => x.IsActive && x.Id == request.Id)?.ToDto<UserEntity, UserDto>()
            ?? throw new LSCoreNotFoundException();
        
        public List<UserDto> GetMultiple(UsersGetMultipleRequest request)
        {
            // ToDo: implement sortable & pageable
            return Queryable().Where(x => x.IsActive)
                .ToDtoList<UserEntity, UserDto>();
        }

        public void UpdateNickname(UsersUpdateNicknameRequest request) =>
            Save(request);

        public UserDto Create(UsersCreateRequest request) =>
            Insert(new UserEntity()
            {
                IsActive = true,
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password),
                Nickname = request.Nickname,
                Type = UserType.User
            }).ToDto<UserEntity, UserDto>();
    }
}
