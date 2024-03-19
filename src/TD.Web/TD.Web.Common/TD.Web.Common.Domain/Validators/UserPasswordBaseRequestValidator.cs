using FluentValidation;
using LSCore.Contracts.Extensions;
using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Helpers.Users;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Common.Repository;

namespace TD.Web.Common.Domain.Validators
{
    public class UserPasswordValidator : AbstractValidator<string>
    {
        private readonly Int16 _passwordMaximumLength = 64;
        private readonly Int16 _passwordMinimumLength = 8;
        public UserPasswordValidator(WebDbContext dbContext)
        {
            RuleFor(password => password)
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
                    if (password.IsPasswordNotStrong())
                    {
                        context.AddFailure(UsersValidationCodes.UVC_010.GetDescription());
                        return;
                    }
                });
        }
    }
}
