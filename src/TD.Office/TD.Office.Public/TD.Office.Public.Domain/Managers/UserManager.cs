using LSCore.Contracts;
using TD.Office.Public.Contracts.Enums.SortColumnCodes;
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
using LSCore.Contracts.Extensions;
using TD.Office.Common.Repository;
using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using TD.Office.Public.Contracts.Dtos.Permissions;

namespace TD.Office.Public.Domain.Managers
{
    public class UserManager (
        ILogger<UserManager> logger,
        OfficeDbContext dbContext,
        IConfigurationRoot configurationRoot,
        LSCoreContextUser contextUser)
        : LSCoreManagerBase<UserManager, UserEntity>(logger, dbContext, contextUser), IUserManager
    {
        public string Login(UsersLoginRequest request)
        {
            request.Validate();

            // I am not checking permissions nor username nor password here
            // because this is handled in validator
            var user = Queryable()
                .FirstOrDefault(x => x.Username.ToUpper() == request.Username!.ToUpper());

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
        
        public LSCoreSortedAndPagedResponse<UserDto> GetMultiple(UsersGetMultipleRequest request) =>
            Queryable().Where(x => x.IsActive)
                .ToSortedAndPagedResponse<UserEntity, UsersSortColumnCodes.Users, UserDto>(request, UsersSortColumnCodes.UsersSortRules);

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

        public List<PermissionDto> GetPermissions(LSCoreIdRequest request)
        {
            var user = Queryable()
                .AsNoTracking()
                .Include(x => x.Permissions)
                .FirstOrDefault(x => x.Id == request.Id);

            if (user == null)
                throw new LSCoreNotFoundException();
            
            
            var allPermissions = Enum.GetValues<Permission>();
            return allPermissions.Select(p => {
                var permission = user.Permissions?.FirstOrDefault(up => up.Permission == p && up.IsActive);
                return new PermissionDto {
                    Name = p.ToString(),
                    Description = p.GetDescription()!,
                    IsGranted = permission != null,
                    Id = (long)p
                };
            }).ToList();
        }

        public void UpdatePermission(UsersUpdatePermissionRequest request)
        {
            request.Validate();
            
            var userPermission = Queryable<UserPermissionEntity>()
                .FirstOrDefault(x => x.UserId == request.Id && x.Permission == request.Permission);
            
            if (userPermission == null)
                Insert(new UserPermissionEntity
                {
                    UserId = request.Id!.Value,
                    Permission = request.Permission!.Value,
                    IsActive = request.IsGranted
                });
            else
            {
                userPermission.IsActive = request.IsGranted;
                Update(userPermission);
            }
        }
    }
}
