using FluentValidation;
using JasperFx.Core;
using System.ComponentModel;
using TD.Core.Contracts.Enums.ValidationCodes;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.Enums.ValidationCodes;
using TD.Web.Contracts.Helpers.Users;
using TD.Web.Contracts.Requests.Users;
using TD.Web.Repository;

namespace TD.Web.Domain.Validators.Users
{
    public class UserRegisterRequestValidator : ValidatorBase<UserRegisterRequest>
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
        private readonly WebDbContext _webDbContext;

        public UserRegisterRequestValidator(WebDbContext dbContext)
        {
            _webDbContext = dbContext;

            RuleFor(x => x.Username)
                .NotNull()
                    .WithMessage(UsersValidationCodes.UVC_001.GetDescription())
                .NotEmpty()
                    .WithMessage(UsersValidationCodes.UVC_001.GetDescription())
                .MinimumLength(_usernameMinimumLength)
                    .WithMessage(string.Format(UsersValidationCodes.UVC_004.GetDescription(String.Empty), _usernameMinimumLength))
                .MaximumLength(_usernameMaximumLength)
                    .WithMessage(string.Format(UsersValidationCodes.UVC_005.GetDescription(String.Empty), _usernameMaximumLength))
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
                    .WithMessage(string.Format(UsersValidationCodes.UVC_008.GetDescription(String.Empty), _passwordMinimumLength))
                .MaximumLength(_passwordMaximumLength)
                    .WithMessage(string.Format(UsersValidationCodes.UVC_009.GetDescription(String.Empty), _passwordMaximumLength))
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
                    .WithMessage(string.Format(UsersValidationCodes.UVC_008.GetDescription(String.Empty), _nicknameMinimumLength))
                .MaximumLength(_nicknameMaximumLength)
                    .WithMessage(string.Format(UsersValidationCodes.UVC_009.GetDescription(String.Empty), _nicknameMaximumLength));

            RuleFor(x => x.DateOfBirth)
                .NotNull()
                    .WithMessage(string.Format(CommonValidationCodes.COMM_002.GetDescription(String.Empty), nameof(UserRegisterRequest.DateOfBirth)));

            RuleFor(x => x.Mobile)
                .NotNull()
                    .WithMessage(string.Format(CommonValidationCodes.COMM_002.GetDescription(String.Empty), nameof(UserRegisterRequest.Mobile)))
                .MaximumLength(_mobileMaximumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_003.GetDescription(String.Empty), nameof(UserRegisterRequest.Mobile), _mobileMaximumLength));

            RuleFor(x => x.Address)
                .NotNull()
                    .WithMessage(string.Format(CommonValidationCodes.COMM_002.GetDescription(String.Empty), nameof(UserRegisterRequest.Address)))
                .MaximumLength(_addressMaximumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_003.GetDescription(String.Empty), nameof(UserRegisterRequest.Address), _addressMaximumLength));

            RuleFor(x => x.Mail)
                .MaximumLength(_mailMaximumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_003.GetDescription(String.Empty), nameof(UserRegisterRequest.Mail), _mailMaximumLength));

            RuleFor(x => x.CityId)
                .NotNull()
                    .WithMessage(string.Format(CommonValidationCodes.COMM_002.GetDescription(String.Empty), nameof(UserRegisterRequest.CityId)))
                .NotEmpty()
                    .WithMessage(string.Format(CommonValidationCodes.COMM_002.GetDescription(String.Empty), nameof(UserRegisterRequest.CityId)));

            RuleFor(x => x.FavoriteStoreId)
                .NotNull()
                    .WithMessage(string.Format(CommonValidationCodes.COMM_002.GetDescription(String.Empty), nameof(UserRegisterRequest.FavoriteStoreId)))
                .NotEmpty()
                    .WithMessage(string.Format(CommonValidationCodes.COMM_002.GetDescription(String.Empty), nameof(UserRegisterRequest.FavoriteStoreId)));
            RuleFor(x => x.Type)
                .NotNull()
                    .WithMessage(string.Format(CommonValidationCodes.COMM_002.GetDescription(String.Empty), nameof(UserRegisterRequest.Type)))
                .NotEmpty()
                    .WithMessage(string.Format(CommonValidationCodes.COMM_002.GetDescription(String.Empty), nameof(UserRegisterRequest.Type)));

        }
    }
}

