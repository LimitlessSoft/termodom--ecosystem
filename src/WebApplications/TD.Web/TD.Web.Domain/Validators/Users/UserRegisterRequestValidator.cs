using FluentValidation;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.Enums.ValidationCodes;
using TD.Web.Contracts.Requests.Users;
using TD.Web.Repository;

namespace TD.Web.Domain.Validators.Users
{
    public class UserRegisterRequestValidator : ValidatorBase<UserRegisterRequest>
    {
        private readonly int _usernameMinimumLength = 6;
        private readonly int _usernameMaximumLength = 32;
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
                    // ToDo: Moze sadrzati samo [0-9A-Za-Z]
                    var user = dbContext.Users.FirstOrDefault(x => x.Username.ToUpper() == username.ToUpper());
                    if (user != null)
                    {
                        context.AddFailure(UsersValidationCodes.UVC_002.GetDescription());
                        return;
                    }
                });

            // ToDo: mora sadrzati barem jedan broj
            // todo: mora sadrzati barem jedan karakter
            // todo: mora imati barem 8 karaktera
            RuleFor(x => x.Password)
                .NotNull()
                    .WithMessage(UsersValidationCodes.UVC_003.GetDescription())
                .NotEmpty()
                    .WithMessage(UsersValidationCodes.UVC_003.GetDescription());

            // todo: nickname validation
            // min 6 karaktera, moze sadrzati bilo sta, max koliko je ubazi
        }
    }
}

