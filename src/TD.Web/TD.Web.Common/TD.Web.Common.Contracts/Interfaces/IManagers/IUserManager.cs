using LSCore.Contracts.Responses;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Admin.Contracts.Requests.Users;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Interfaces.IManagers;

public interface IUserManager
{
    string Login(UserLoginRequest request);
    void Register(UserRegisterRequest request);
    void PromoteUser(UserPromoteRequest request);
    void SetUserProductPriceGroupLevel(SetUserProductPriceGroupLevelRequest request);
    void MarkLastSeen();
    UserInformationDto Me();
    LSCoreSortedAndPagedResponse<UsersGetDto> GetUsers(UsersGetRequest request);
    GetSingleUserDto GetSingleUser(GetSingleUserRequest request);
    List<UserProductPriceLevelsDto> GetUserProductPriceLevels(GetUserProductPriceLevelsRequest request);
    void UpdateUser(UpdateUserRequest request);
    void PutUserProductPriceLevel(PutUserProductPriceLevelRequest request);
    void PutUserType(PutUserTypeRequest request);
    void PutUserStatus(PutUserStatusRequest request);
    void GetOwnership(GetOwnershipRequest request);
    void ApproveUser(ApproveUserRequest request);
    void ChangeUserPassword(ChangeUserPasswordRequest request);
    // This is one time method used to fix mobile numbers in database
    // string FixMobiles();
    void ResetPassword(UserResetPasswordRequest request);
    Task SendBulkSms(SendBulkSmsRequest request);
    void SetPassword(UserSetPasswordRequest request);
    UsersAnalyzeOrderedProductsDto AnalyzeOrderedProducts(UsersAnalyzeOrderedProductsRequest request);
    bool HasPermission(Permission permission);
}