using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Helpers.Users;
using TD.Web.Common.Contracts.Interfaces;
using LSCore.Contracts.Extensions;
using TD.Web.Common.Repository;
using FluentValidation;

// ReSharper disable RedundantJumpStatement

namespace TD.Web.Common.Domain.Validators.Users
{
    public abstract class UserPasswordValidatorBase<TRequest> : AbstractValidator<TRequest>
        where TRequest : class, IPassword
    {

        private readonly short _passwordMaximumLength = 64;
        private readonly short _passwordMinimumLength = 8;

        public UserPasswordValidatorBase(WebDbContext dbContext)
        {
            RuleFor(request => request.Password)
                .NotNull()
                    .WithMessage(UsersValidationCodes.UVC_003.GetDescription())
                .NotEmpty()
                    .WithMessage(UsersValidationCodes.UVC_003.GetDescription())
                .MinimumLength(_passwordMinimumLength)
                    .WithMessage(string.Format(UsersValidationCodes.UVC_008.GetDescription()!, _passwordMinimumLength))
                .MaximumLength(_passwordMaximumLength)
                    .WithMessage(string.Format(UsersValidationCodes.UVC_009.GetDescription()!, _passwordMaximumLength))
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