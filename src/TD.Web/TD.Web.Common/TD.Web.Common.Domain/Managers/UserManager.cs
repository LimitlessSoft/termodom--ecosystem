using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Omu.ValueInjecter;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
using LSCore.Domain.Extensions;
using TD.Web.Common.Contracts.Helpers;

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

            var qUserResponse = Queryable()
                .LSCoreFilters(x => x.IsActive && x.Username.ToUpper() == request.Username.ToUpper());
            response.Merge(qUserResponse);
            if (response.NotOk)
                return response;

            var user = qUserResponse.Payload!
                .AsNoTrackingWithIdentityResolution()
                .First();
            return new LSCoreResponse<string>(GenerateJSONWebToken(user));
        }

        public LSCoreResponse Register(UserRegisterRequest request)
        {
            var response = new LSCoreResponse();
            request.Mobile = MobilePhoneHelpers.GenarateValidNumber(request.Mobile);

            if (request.IsRequestInvalid(response))
                return response;

            var professionResponse = First<ProfessionEntity>(x => x.IsActive);
            response.Merge(professionResponse);
            if (response.NotOk)
                return response;

            var user = new UserEntity();
            user.InjectFrom(request);
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.Type = UserType.User;
            user.ProfessionId = professionResponse.Payload!.Id;

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
                .Where(x =>
                    x.Id != 0 &&
                    (request.HasReferent == null || (x.Referent != null) == request.HasReferent) &&
                    (request.IsActive == null || x.IsActive == request.IsActive)
                )
                .ToSortedAndPagedResponse(request, UsersSortColumnCodes.UsersSortRules);

            response.Merge(users);
            if (response.NotOk)
                return response;

            return new LSCoreSortedPagedResponse<UsersGetDto>(users.Payload!.ToDtoList<UsersGetDto, UserEntity>(),
                request,
                users.Pagination.TotalElementsCount);
        }

        public LSCoreResponse<GetSingleUserDto> GetSingleUser(GetSingleUserRequest request)
        {
            var response = new LSCoreResponse<GetSingleUserDto>();

            var qResponse = Queryable();

            response.Merge(qResponse);
            if (response.NotOk) 
                return response;

            var user = qResponse.Payload!
                .Include(x => x.Profession)
                .Include(x => x.City)
                .Include(x => x.FavoriteStore)
                .Include(x => x.Referent)
                .FirstOrDefault(x => string.Equals(x.Username, request.Username));

            if (user == null)
                return LSCoreResponse<GetSingleUserDto>.NotFound();

            response.Payload = user.ToDto<GetSingleUserDto, UserEntity>();
            response.Payload.AmIOwner = user.ReferentId != null && user.ReferentId == CurrentUser!.Id;
            return response;
        }

        public LSCoreListResponse<UserProductPriceLevelsDto> GetUserProductPriceLevels(GetUserProductPriceLevelsRequest request)
        {
            var response = new LSCoreListResponse<UserProductPriceLevelsDto>();

            var qResponse = Queryable();
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            var levels = qResponse.Payload!
                .Include(x => x.ProductPriceGroupLevels)
                .Where(x => x.Id == request.UserId)
                .FirstOrDefault();

            if(levels == null)
                return LSCoreListResponse<UserProductPriceLevelsDto>.BadRequest();

            var groupsResponse = Queryable<ProductPriceGroupEntity>();

            response.Merge(groupsResponse);
            if (response.NotOk)
                return response;

            var groups = groupsResponse.Payload!
                .Where(x => x.IsActive)
                .ToList();

            response.Payload = levels.ProductPriceGroupLevels.ToUserPriceLevelsDto(groups);
            return response;
        }

        public LSCoreResponse UpdateUser(UpdateUserRequest request)
        {
            request.Mobile = MobilePhoneHelpers.GenarateValidNumber(request.Mobile);
            return new LSCoreResponse(Save(request));
        }

        public LSCoreResponse PutUserProductPriceLevel(PutUserProductPriceLevelRequest request)
        {
            var response = new LSCoreResponse();

            var priceLevelResponse = First<ProductPriceGroupLevelEntity>(x =>
                x.IsActive && x.UserId == request.UserId && x.ProductPriceGroupId == request.ProductPriceGroupId);
            if (priceLevelResponse.Status == HttpStatusCode.NotFound)
            {
                response.Merge(Insert<ProductPriceGroupLevelEntity>(new ProductPriceGroupLevelEntity()
                {
                    UserId = request.UserId,
                    ProductPriceGroupId = request.ProductPriceGroupId,
                    Level = request.Level
                }));
                return response;
            }
            else
            {
                response.Merge(priceLevelResponse);
                if (response.NotOk)
                    return response;

                var priceLevel = priceLevelResponse.Payload!;
                priceLevel.Level = request.Level;
                response.Merge(Update(priceLevel));
                return response;
            }
        }

        public LSCoreResponse PutUserType(PutUserTypeRequest request)
        {
            var response = new LSCoreResponse();

            var userResponse = First(x => x.Username == request.Username && x.IsActive);
            response.Merge(userResponse);
            if (response.NotOk)
                return response;
            
            var user = userResponse.Payload!;
            user.Type = request.Type;
            response.Merge(Update(user));
            return response;
        }

        public LSCoreResponse PutUserStatus(PutUserStatusRequest request)
        {
            var response = new LSCoreResponse();

            var userResponse = First(x => x.Username == request.Username);
            response.Merge(userResponse);
            if (response.NotOk)
                return response;

            var user = userResponse.Payload!;
            user.IsActive = request.IsActive;
            response.Merge(Update(user));
            return response;
        }

        public LSCoreResponse GetOwnership(GetOwnershipRequest request)
        {
            var response = new LSCoreResponse();

            var userResponse = First(x => x.Username == request.Username);
            response.Merge(userResponse);
            if (response.NotOk)
                return response;

            var user = userResponse.Payload!;
            user.ReferentId = CurrentUser!.Id;
            response.Merge(Update(user));
            return response;
        }

        public LSCoreResponse ApproveUser(ApproveUserRequest request)
        {
            var response = new LSCoreResponse();

            var userResponse = First(x => x.Username == request.Username);
            response.Merge(userResponse);
            if (response.NotOk)
                return response;

            var user = userResponse.Payload!;
            user.ProcessingDate = DateTime.UtcNow;
            user.IsActive = true;
            response.Merge(Update(user));
            return response;
        }

        public LSCoreResponse ChangeUserPassword(ChangeUserPasswordRequest request)
        {
            var response = new LSCoreResponse();

            if (request.IsRequestInvalid(response))
                return response;

            var userResponse = First(x => x.Username == request.Username);
            response.Merge(userResponse);
            if (response.NotOk)
                return response;

            var user = userResponse.Payload!;
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
            response.Merge(Update(user));

            return response;
        }
    }
}
