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
using TD.OfficeServer.Contracts.Requests.SMS;
using TD.Web.Admin.Contracts.Requests.Users;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Common.Contracts.Helpers.Users;

namespace TD.Web.Common.Domain.Managers
{
    public class UserManager : LSCoreBaseManager<UserManager, UserEntity>, IUserManager
    {
        private readonly IConfigurationRoot _configurationRoot;
        private readonly IOfficeServerApiManager _officeServerApiManager;
        private readonly ILogger<UserManager> _logger;
        public UserManager(IConfigurationRoot configurationRoot, ILogger<UserManager> logger, WebDbContext dbContext, IOfficeServerApiManager officeServerApiManager)
            : base(logger, dbContext)
        {
            _configurationRoot = configurationRoot;
            _officeServerApiManager = officeServerApiManager;
            _logger = logger;
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
            request.SortColumn = UsersSortColumnCodes.Users.Id;

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

            _officeServerApiManager.SMSQueueAsync(new SMSQueueRequest()
            {
                Numbers = new List<string>()
                {
                    user.Mobile
                },
                Text = $"{user.Nickname}, Vasa lozinka je promenjena na {request.Password}. Lozinku u svakom trenutku mozete promeniti u delu Moj Kutak."
            });

            return response;
        }

        public LSCoreResponse ResetPassword(UserResetPasswordRequest request)
        {
            var response = new LSCoreResponse();
            var userResponse = First(x => x.IsActive && x.Username.ToLower() == request.Username.ToLower());
            if (userResponse.Status == HttpStatusCode.NotFound)
                return response;
            
            response.Merge(userResponse);
            if (response.NotOk)
                return response;
            
            var user = userResponse.Payload!;
            if (MobilePhoneHelpers.GenarateValidNumber(user.Mobile) != MobilePhoneHelpers.GenarateValidNumber(request.Mobile))
                return response;

            string rawPassword = UsersHelpers.GenerateNewPassword();
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(rawPassword);
            response.Merge(Update(user));

            if (response.NotOk)
                return response;

            _officeServerApiManager.SMSQueueAsync(new SMSQueueRequest()
            {
                Numbers = new List<string>()
                {
                    user.Mobile
                },
                Text = user.Nickname + ", Vasa nova lozinka je: " + rawPassword + ". U svakom trenutku samostalno mozete promeniti lozinku u delu Moj Kutak."
            });
            
            return response;
        }

        public async Task<LSCoreResponse> SendBulkSms(SendBulkSmsRequest request)
        {
            var qUsers = Queryable()
                .LSCoreFilters(x => x.IsActive
                    && (request.FavoriteStoreId == null || request.FavoriteStoreId == x.FavoriteStoreId)
                    && (request.CityId == null || request.CityId == x.CityId)
                    && (request.ProfessionId == null || request.ProfessionId == x.ProfessionId)
                    && (request.UserTypeId == null || request.UserTypeId == (int)x.Type)
                    && (request.IsActive == null || request.IsActive == x.IsActive));
            
            if(qUsers.NotOk)
                return LSCoreResponse.BadRequest();

            var users = qUsers.Payload;
            
            var mobilePhones = users!.Select(x => x.Mobile).ToList();
            await _officeServerApiManager.SMSQueueAsync(new SMSQueueRequest()
            {
                Numbers = mobilePhones,
                Text = request.Text
            });
            return new LSCoreResponse();
        }

        public LSCoreResponse SetPassword(UserSetPasswordRequest request)
        {
            if (CurrentUser == null)
                return LSCoreResponse.BadRequest();

            var response = new LSCoreResponse();
            
            var userResponse = First(x => x.Username == CurrentUser.Username);
            response.Merge(userResponse);
            if (response.NotOk)
                return response;
            
            var user = userResponse.Payload!;
            if (!BCrypt.Net.BCrypt.EnhancedVerify(request.OldPassword, user.Password))
                return LSCoreResponse.BadRequest("Stara lozinka nije ispravna");
            
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
            response.Merge(Update(user));
            return response;
        }

        public LSCoreResponse<UsersAnalyzeOrderedProductsDto> AnalyzeOrderedProducts(UsersAnalyzeOrderedProductsRequest request)
        {
            var response = new LSCoreResponse<UsersAnalyzeOrderedProductsDto>();

            request.IsRequestInvalid(response);
            
            var qrOrders = Queryable<OrderEntity>();
            response.Merge(qrOrders);
            if (response.NotOk)
                return response;

            var dateFromUtc = request.Range switch
            {
                UsersAnalyzeOrderedProductsRange.Last30Days => DateTime.UtcNow.AddDays(-30),
                UsersAnalyzeOrderedProductsRange.LastYear => DateTime.UtcNow.AddYears(-1),
                UsersAnalyzeOrderedProductsRange.SinceCreation => new DateTime(),
                UsersAnalyzeOrderedProductsRange.ThisYear => new DateTime(DateTime.UtcNow.Year, 1, 1),
                _ => DateTime.UtcNow
            };

            var qOrders = qrOrders.Payload;

            var orders = qOrders
                .Include(x => x.Items)
                .Include(x => x.User)
                .Where(x => x.IsActive
                            && x.User.Username == request.Username
                            && x.CheckedOutAt >= dateFromUtc);
            
            // Get sum of items.quantity from orders grouped by item.productId
            var products = orders
                .SelectMany(x => x.Items)
                .Where(x => x.IsActive)
                .Select(x => x.ProductId)
                .Distinct()
                .ToList();

            response.Payload = new UsersAnalyzeOrderedProductsDto();
            foreach (var productId in products)
            {
                var rProduct = First<ProductEntity>(x => x.Id == productId);
                response.Merge(rProduct);
                if (response.NotOk)
                    return response;
                
                try
                {
                    response.Payload.Items.Add(new UsersAnalyzeOrderedProductsItemDto()
                    {
                        Id = productId,
                        Name = rProduct.Payload.Name,
                        ValueSum = orders
                            .SelectMany(x => x.Items)
                            .Where(x => x.IsActive && x.ProductId == productId)
                            .Sum(x => x.Price * x.Quantity),
                        DiscountSum = orders
                            .SelectMany(x => x.Items)
                            .Where(x => x.IsActive && x.ProductId == productId)
                            .Sum(x => (x.PriceWithoutDiscount - x.Price) * x.Quantity),
                        QuantitySum = orders
                            .SelectMany(x => x.Items)
                            .Where(x => x.IsActive && x.ProductId == productId)
                            .Sum(x => x.Quantity)
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
            }

            return response;
        }

        // This is one time method used to fix mobile numbers in database
        // public string FixMobiles()
        // {
        //     var qUsers = Queryable<UserEntity>();
        //     if (qUsers.NotOk)
        //         return "Error";
        //     
        //     var users = qUsers.Payload!.ToList();
        //     foreach (var user in users.OrderBy(x => x.Id))
        //     {
        //         if(string.IsNullOrWhiteSpace(user.Mobile))
        //             continue;
        //         
        //         user.Mobile = MobilePhoneHelpers.GenarateValidNumber(user.Mobile);
        //         Update(user);
        //     }
        //
        //     return "success";
        // }
    }
}
