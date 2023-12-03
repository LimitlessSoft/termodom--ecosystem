using FluentValidation;
using LSCore.Contracts.Enums.ValidationCodes;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Common.Repository;
using TD.Web.Common.Contracts.Helpers.Users;

namespace TD.Web.Common.Domain.Validators.Users
{
    public class UserRegisterRequestValidator : LSCoreValidatorBase<UserRegisterRequest>
    {
        private readonly int _usernameMinimumLength = 6;
        private readonly int _usernameMaximumLength = 32;
        private readonly int _passwordMaximumLength = 64;
        private readonly int _passwordMinimumLength = 8;
        private readonly int _nicknameMinimumLength = 6;
        private readonly int _nicknameMaximumLength = 32;
        private readonly int _mobileMaximumLength = 16;
        private readonly int _addressMaximumLength = 32;
        private readonly int _mailMaximumLength = 32;
        private readonly int _minAge = 18;
        private readonly int _maxAge = 70;

        public UserRegisterRequestValidator(WebDbContext dbContext)
        {

            RuleFor(x => x.Username)
                .NotNull()
                    .WithMessage(UsersValidationCodes.UVC_001.GetDescription())
                .NotEmpty()
                    .WithMessage(UsersValidationCodes.UVC_001.GetDescription())
                .MinimumLength(_usernameMinimumLength)
                    .WithMessage(string.Format(UsersValidationCodes.UVC_004.GetDescription(), _usernameMinimumLength))
                .MaximumLength(_usernameMaximumLength)
                    .WithMessage(string.Format(UsersValidationCodes.UVC_005.GetDescription(), _usernameMaximumLength))
                .Custom((username, context) =>
                {
                    if(username.IsUsernameNotValid())
                    {
                        context.AddFailure(UsersValidationCodes.UVC_007.GetDescription());
                        return;
                    }
                    var user = dbContext.Users.FirstOrDefault(x => x.Username.ToUpper() == username.ToUpper());
                    if (user != null)
                    {
                        context.AddFailure(UsersValidationCodes.UVC_002.GetDescription());
                        return;
                    }
                });

            RuleFor(x => x.Password)
                .NotNull()
                    .WithMessage(UsersValidationCodes.UVC_003.GetDescription())
                .NotEmpty()
                    .WithMessage(UsersValidationCodes.UVC_003.GetDescription())
                .MinimumLength(_passwordMinimumLength)
                    .WithMessage(string.Format(UsersValidationCodes.UVC_008.GetDescription(), _passwordMinimumLength))
                .MaximumLength(_passwordMaximumLength)
                    .WithMessage(string.Format(UsersValidationCodes.UVC_009.GetDescription(), _passwordMaximumLength))
                .Custom((password, context) =>
                 {
                     if(password.IsPasswordNotStrong())
                     {
                         context.AddFailure(UsersValidationCodes.UVC_010.GetDescription());
                         return;
                     }
                 });

            RuleFor(x => x.Nickname)
                .NotNull()
                    .WithMessage(UsersValidationCodes.UVC_011.GetDescription())
                .NotEmpty()
                    .WithMessage(UsersValidationCodes.UVC_011.GetDescription())
                .MinimumLength(_nicknameMinimumLength)
                    .WithMessage(string.Format(UsersValidationCodes.UVC_008.GetDescription(), _nicknameMinimumLength))
                .MaximumLength(_nicknameMaximumLength)
                    .WithMessage(string.Format(UsersValidationCodes.UVC_009.GetDescription(), _nicknameMaximumLength));

            RuleFor(x => x.DateOfBirth)
                .NotNull()
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_002.GetDescription(), nameof(UserRegisterRequest.DateOfBirth)))
                .Custom((dateOfBirth, context) =>
                {
                    var age = DateTime.Now.Year - dateOfBirth.Year;
                    if(age < _minAge || age > _maxAge)
                    {
                        context.AddFailure(UsersValidationCodes.UVC_014.GetDescription());
                        return;
                    }
                });

            RuleFor(x => x.Mobile)
                .NotNull()
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_002.GetDescription(), nameof(UserRegisterRequest.Mobile)))
                .MaximumLength(_mobileMaximumLength)
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_003.GetDescription(), nameof(UserRegisterRequest.Mobile), _mobileMaximumLength));

            RuleFor(x => x.Address)
                .NotNull()
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_002.GetDescription(), nameof(UserRegisterRequest.Address)))
                .MaximumLength(_addressMaximumLength)
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_003.GetDescription(), nameof(UserRegisterRequest.Address), _addressMaximumLength));

            RuleFor(x => x.Mail)
                .MaximumLength(_mailMaximumLength)
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_003.GetDescription(), nameof(UserRegisterRequest.Mail), _mailMaximumLength));

            RuleFor(x => x.CityId)
                .NotNull()
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_002.GetDescription(), nameof(UserRegisterRequest.CityId)))
                .NotEmpty()
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_002.GetDescription(), nameof(UserRegisterRequest.CityId)));

            RuleFor(x => x.FavoriteStoreId)
                .NotNull()
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_002.GetDescription(), nameof(UserRegisterRequest.FavoriteStoreId)))
                .NotEmpty()
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_002.GetDescription(), nameof(UserRegisterRequest.FavoriteStoreId)));

        }
    }
}

