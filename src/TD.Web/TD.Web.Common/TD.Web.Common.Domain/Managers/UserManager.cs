using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Omu.ValueInjecter;
using System.IdentityModel.Tokens.Jwt;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;
using TD.Web.Common.Contracts.Enums;
using Microsoft.EntityFrameworkCore;
using LSCore.Domain.Managers;
using LSCore.Contracts.Http;
using LSCore.Domain.Validators;
using LSCore.Contracts.Extensions;
using Microsoft.IdentityModel.Tokens;
using LSCore.Contracts;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.DtoMappings.Users;
using LSCore.Contracts.Responses;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;

namespace TD.Web.Common.Domain.Managers
{
    public class UserManager : LSCoreBaseManager<UserManager, UserEntity>, IUserManager
    {
        private readonly IConfigurationRoot _configurationRoot;
        public UserManager(IConfigurationRoot configurationRoot, ILogger<UserManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
            _configurationRoot = configurationRoot;
        }

        private string GenerateJSONWebToken(UserEntity user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationRoot["JWT_KEY"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(LSCoreContractsConstants.ClaimNames.CustomUsername, user.Username),
                new Claim(LSCoreContractsConstants.ClaimNames.CustomUserId, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Type.ToString()),
                new Claim("TestPolicyPermission", "true")
            };

            #region Generate JWT token
            var jwtIssuer = _configurationRoot["JWT_ISSUER"];
            var jwtAudience = _configurationRoot["JWT_AUDIENCE"];
            var token = new JwtSecurityToken(jwtIssuer, jwtAudience,
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);
            #endregion

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public LSCoreResponse<string> Login(UserLoginRequest request)
        {
            var response = new LSCoreResponse<string>();

            if (request.IsRequestInvalid(response))
                return response;

            var userResponse = First(x => x.Username.ToUpper() == request.Username.ToUpper());
            response.Merge(userResponse);
            if (response.NotOk)
                return response;

            return new LSCoreResponse<string>(GenerateJSONWebToken(userResponse.Payload));
        }

        public LSCoreResponse Register(UserRegisterRequest request)
        {
            var response = new LSCoreResponse();

            if (request.IsRequestInvalid(response))
                return response;

            var user = new UserEntity();
            user.InjectFrom(request);
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.Type = UserType.User;

            response.Merge(Insert(user));
            return response;
        }

        public LSCoreResponse MarkLastSeen() =>
            new LSCoreResponse(Save(new UserSaveLastTimeSeenRequest(CurrentUser.Id)));

        public LSCoreResponse PromoteUser(UserPromoteRequest request) =>
            new LSCoreResponse(Save(request));

        public LSCoreResponse SetUserProductPriceGroupLevel(SetUserProductPriceGroupLevelRequest request)
        {
            var response = new LSCoreResponse();
            if (request.IsRequestInvalid(response))
                return response;

            var qResponse = Queryable(x => x.IsActive && x.Id == request.Id);
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            var userEntity =
                qResponse.Payload!
                .Include(x => x.ProductPriceGroupLevels)
                .FirstOrDefault();
            if (userEntity == null)
                return LSCoreResponse.NotFound();

            var productPriceGroupLevelEntity = userEntity.ProductPriceGroupLevels.FirstOrDefault(x => x.ProductPriceGroupId == request.ProductPriceGroupId);

            if (productPriceGroupLevelEntity != null)
                productPriceGroupLevelEntity.Level = request.Level.Value;
            else
                userEntity.ProductPriceGroupLevels.Add(new ProductPriceGroupLevelEntity()
                {
                    UserId = userEntity.Id,
                    Level = request.Level.Value,
                    ProductPriceGroupId = request.ProductPriceGroupId.Value
                });

            response.Merge(Update(userEntity));
            return response;
        }

        public LSCoreResponse<UserInformationDto> Me() =>
            new LSCoreResponse<UserInformationDto>(First(x => CurrentUser != null && x.Id == CurrentUser.Id && x.IsActive).Payload.ToUserInformationDto());

        public LSCoreSortedPagedResponse<UsersGetDto> GetUsers(UsersGetRequest request)
        {
            var response = new LSCoreSortedPagedResponse<UsersGetDto>();

            var qResponse = Queryable();

            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            var users = qResponse.Payload!
                .ToSortedAndPagedResponse(request, UsersSortColumnCodes.UsersSortRules);

            response.Merge(users);
            if (response.NotOk)
                return response;


            return response;
        }
    }
}
