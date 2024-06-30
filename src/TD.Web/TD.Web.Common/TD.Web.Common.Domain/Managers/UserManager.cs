using TD.Web.Common.Contracts.Enums.SortColumnCodes;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.DtoMappings.Users;
using TD.OfficeServer.Contracts.Requests.SMS;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Admin.Contracts.Requests.Users;
using TD.Web.Common.Contracts.Helpers.Users;
using TD.Web.Common.Contracts.Dtos.Users;
using Microsoft.Extensions.Configuration;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Helpers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using TD.Web.Common.Contracts.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Exceptions;
using LSCore.Domain.Extensions;
using TD.Web.Common.Repository;
using System.Security.Claims;
using LSCore.Domain.Managers;
using Omu.ValueInjecter;
using LSCore.Contracts;
using System.Text;
using LSCore.Contracts.Responses;
using Microsoft.AspNetCore.Http;

namespace TD.Web.Common.Domain.Managers;

public class UserManager (
    IConfigurationRoot configurationRoot,
    ILogger<UserManager> logger,
    WebDbContext dbContext,
    IOfficeServerApiManager officeServerApiManager, LSCoreContextUser contextUser)
    : LSCoreManagerBase<UserManager, UserEntity>(logger, dbContext, contextUser), IUserManager
{
    private readonly ILogger<UserManager> _logger = logger;

    private string GenerateJsonWebToken(UserEntity user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationRoot["JWT_KEY"]!));
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
        var jwtIssuer = configurationRoot["JWT_ISSUER"];
        var jwtAudience = configurationRoot["JWT_AUDIENCE"];
        var token = new JwtSecurityToken(jwtIssuer, jwtAudience,
            claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);
        #endregion

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string Login(UserLoginRequest request)
    {
        request.Validate();

        var user = Queryable()
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefault(x => x.IsActive
                                 && x.Username.ToUpper() == request.Username.ToUpper());

        if (user == null)
            throw new LSCoreUnauthenticatedException();

        return new string(GenerateJsonWebToken(user));
    }

    public void Register(UserRegisterRequest request)
    {
        request.Mobile = MobilePhoneHelpers.GenarateValidNumber(request.Mobile);

        request.Validate();

        var profession = Queryable<ProfessionEntity>()
            .Where(x => x.IsActive);

        var user = new UserEntity();
        user.InjectFrom(request);
        user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
        user.CreatedAt = DateTime.UtcNow;
        user.Type = UserType.User;
        user.ProfessionId = profession.FirstOrDefault()!.Id; // TODO: It always gets first profession, should be changed

        Insert(user);
    }

    public void MarkLastSeen() =>
        Save(new UserSaveLastTimeSeenRequest(CurrentUser!.Id!.Value));

    public void PromoteUser(UserPromoteRequest request) =>
        Save(request);

    public void SetUserProductPriceGroupLevel(SetUserProductPriceGroupLevelRequest request)
    {
        request.Validate();

        var user = Queryable()
            .Include(x => x.ProductPriceGroupLevels)
            .FirstOrDefault(x => x.IsActive && x.Id == request.Id);
        
        if (user == null)
            throw new LSCoreNotFoundException();

        var productPriceGroupLevelEntity = user.ProductPriceGroupLevels.FirstOrDefault(x => x.ProductPriceGroupId == request.ProductPriceGroupId);

        if (productPriceGroupLevelEntity != null)
            productPriceGroupLevelEntity.Level = request.Level!.Value;
        else
            user.ProductPriceGroupLevels.Add(new ProductPriceGroupLevelEntity()
            {
                UserId = user.Id,
                Level = request.Level!.Value,
                ProductPriceGroupId = request.ProductPriceGroupId!.Value
            });

        Update(user);
    }

    public UserInformationDto Me() =>
        Queryable().FirstOrDefault(x => CurrentUser != null && x.Id == CurrentUser.Id && x.IsActive)
            .ToUserInformationDto();

    public LSCoreSortedAndPagedResponse<UsersGetDto> GetUsers(UsersGetRequest request)
    {
        request.SortColumn = UsersSortColumnCodes.Users.Id; // TODO: This is fixed to ID

        return Queryable()
            .Where(x =>
                x.Id != 0 &&
                (request.HasReferent == null || (x.Referent != null) == request.HasReferent) &&
                (request.IsActive == null || x.IsActive == request.IsActive)
            )
            .ToSortedAndPagedResponse<UserEntity, UsersSortColumnCodes.Users, UsersGetDto>(request, UsersSortColumnCodes.UsersSortRules);
    }

    public GetSingleUserDto GetSingleUser(GetSingleUserRequest request)
    {
        var user = Queryable()
            .Include(x => x.Profession)
            .Include(x => x.City)
            .Include(x => x.FavoriteStore)
            .Include(x => x.Referent)
            .FirstOrDefault(x => string.Equals(x.Username, request.Username));

        if (user == null)
            throw new LSCoreNotFoundException();

        var dto = user.ToDto<UserEntity, GetSingleUserDto>();
        dto.AmIOwner = user.ReferentId != null && user.ReferentId == CurrentUser!.Id;
        return dto;
    }

    public List<UserProductPriceLevelsDto> GetUserProductPriceLevels(GetUserProductPriceLevelsRequest request)
    {
        var user = Queryable()
            .Include(x => x.ProductPriceGroupLevels)
            .FirstOrDefault(x => x.Id == request.UserId);

        if(user == null)
            throw new LSCoreNotFoundException();

        var groups = Queryable<ProductPriceGroupEntity>()
            .Where(x => x.IsActive)
            .ToList();

        return user.ProductPriceGroupLevels.ToUserPriceLevelsDto(groups);
    }

    public void UpdateUser(UpdateUserRequest request)
    {
        request.Mobile = MobilePhoneHelpers.GenarateValidNumber(request.Mobile);
        Save(request);
    }

    public void PutUserProductPriceLevel(PutUserProductPriceLevelRequest request)
    {
        var priceLevel = Queryable
                <ProductPriceGroupLevelEntity>()
            .FirstOrDefault(x => x.IsActive && x.UserId == request.UserId && x.ProductPriceGroupId == request.ProductPriceGroupId);

        if (priceLevel == null)
        {
            Insert(new ProductPriceGroupLevelEntity()
            {
                UserId = request.UserId,
                ProductPriceGroupId = request.ProductPriceGroupId,
                Level = request.Level
            });
            return;
        }

        priceLevel.Level = request.Level;
        Update(priceLevel);
    }

    public void PutUserType(PutUserTypeRequest request)
    {
        var user = Queryable()
            .FirstOrDefault(x => x.Username == request.Username && x.IsActive);
        
        if (user == null)
            throw new LSCoreNotFoundException();
            
        user.Type = request.Type;
        Update(user);
    }

    public void PutUserStatus(PutUserStatusRequest request)
    {
        var user = Queryable()
            .FirstOrDefault(x => x.Username == request.Username);
        
        if (user == null)
            throw new LSCoreNotFoundException();
        
        user.IsActive = request.IsActive;
        Update(user);
    }

    public void GetOwnership(GetOwnershipRequest request)
    {
        var user = Queryable()
            .FirstOrDefault(x => x.Username == request.Username);
        
        if (user == null)
            throw new LSCoreNotFoundException();

        user.ReferentId = CurrentUser!.Id;
        Update(user);
    }

    public void ApproveUser(ApproveUserRequest request)
    {
        var user = Queryable()
            .FirstOrDefault(x => x.Username == request.Username);
        
        if (user == null)
            throw new LSCoreNotFoundException();

        user.ProcessingDate = DateTime.UtcNow;
        user.IsActive = true;
        Update(user);
    }

    public void ChangeUserPassword(ChangeUserPasswordRequest request)
    {
        request.Validate();

        var user = Queryable()
            .FirstOrDefault(x => x.Username == request.Username);
        
        if (user == null)
            throw new LSCoreNotFoundException();

        user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
        Update(user);

        officeServerApiManager.SmsQueueAsync(new SMSQueueRequest()
        {
            Numbers = [user.Mobile],
            Text = $"{user.Nickname}, Vasa lozinka je promenjena na {request.Password}. Lozinku u svakom trenutku mozete promeniti u delu Moj Kutak."
        });
    }

    public void ResetPassword(UserResetPasswordRequest request)
    {
        var user = Queryable()
            .FirstOrDefault(x => x.IsActive && x.Username.ToLower() == request.Username.ToLower());

        if (user == null)
            return;
            
        if (MobilePhoneHelpers.GenarateValidNumber(user.Mobile) != MobilePhoneHelpers.GenarateValidNumber(request.Mobile))
            return;

        var rawPassword = UsersHelpers.GenerateNewPassword();
        user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(rawPassword);
        Update(user);

        officeServerApiManager.SmsQueueAsync(new SMSQueueRequest()
        {
            Numbers = [user.Mobile],
            Text = user.Nickname + ", Vasa nova lozinka je: " + rawPassword + ". U svakom trenutku samostalno mozete promeniti lozinku u delu Moj Kutak. https://termodom.rs"
        });
    }

    public async Task SendBulkSms(SendBulkSmsRequest request)
    {
        var users = Queryable()
            .Where(x => x.IsActive
                                && (request.FavoriteStoreId == null || request.FavoriteStoreId == x.FavoriteStoreId)
                                && (request.CityId == null || request.CityId == x.CityId)
                                && (request.ProfessionId == null || request.ProfessionId == x.ProfessionId)
                                && (request.UserTypeId == null || request.UserTypeId == (int)x.Type)
                                && (request.IsActive == null || request.IsActive == x.IsActive));

            
        var mobilePhones = users.Select(x => x.Mobile).ToList();
        await officeServerApiManager.SmsQueueAsync(new SMSQueueRequest()
        {
            Numbers = mobilePhones,
            Text = request.Text
        });
    }

    public void SetPassword(UserSetPasswordRequest request)
    {
        if (CurrentUser?.Id == null)
            throw new LSCoreBadRequestException();

        var user = Queryable()
            .FirstOrDefault(x => x.Id == CurrentUser.Id);
        
        if (user == null)
            throw new LSCoreNotFoundException();

        if (!BCrypt.Net.BCrypt.EnhancedVerify(request.OldPassword, user.Password))
            throw new LSCoreBadRequestException("Stara lozinka nije ispravna");
            
        user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
        Update(user);
    }

    public UsersAnalyzeOrderedProductsDto AnalyzeOrderedProducts(UsersAnalyzeOrderedProductsRequest request)
    {
        request.Validate();

        var dateFromUtc = request.Range switch
        {
            UsersAnalyzeOrderedProductsRange.Last30Days => DateTime.UtcNow.AddDays(-30),
            UsersAnalyzeOrderedProductsRange.LastYear => DateTime.UtcNow.AddYears(-1),
            UsersAnalyzeOrderedProductsRange.SinceCreation => new DateTime(),
            UsersAnalyzeOrderedProductsRange.ThisYear => new DateTime(DateTime.UtcNow.Year, 1, 1),
            _ => DateTime.UtcNow
        };
            
        var orders = Queryable<OrderEntity>()
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
        
        var dto = new UsersAnalyzeOrderedProductsDto();

        foreach (var productId in products)
        {
            var product = Queryable<ProductEntity>()
                .FirstOrDefault(x => x.Id == productId);
            
            if (product == null)
                continue;
                
            try
            {
                dto.Items.Add(new UsersAnalyzeOrderedProductsItemDto()
                {
                    Id = productId,
                    Name = product.Name,
                    ValueSum = orders
                        .SelectMany(x => x.Items)
                        .Where(x => x.IsActive && x.ProductId == productId)
                        .ToList()
                        .Sum(x => x.Price * x.Quantity),
                    DiscountSum = orders
                        .SelectMany(x => x.Items)
                        .Where(x => x.IsActive && x.ProductId == productId)
                        .ToList()
                        .Sum(x => (x.PriceWithoutDiscount - x.Price) * x.Quantity),
                    QuantitySum = orders
                        .SelectMany(x => x.Items)
                        .Where(x => x.IsActive && x.ProductId == productId)
                        .ToList()
                        .Sum(x => x.Quantity)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        
        return dto;
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