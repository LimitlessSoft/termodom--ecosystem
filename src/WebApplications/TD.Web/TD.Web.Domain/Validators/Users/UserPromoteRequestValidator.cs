using FluentValidation;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.Enums;
using TD.Web.Contracts.Enums.ValidationCodes;
using TD.Web.Contracts.Requests.Users;
using TD.Web.Repository;

namespace TD.Web.Domain.Validators.Users
{
    public class UserPromoteRequestValidator : ValidatorBase<UserPromoteRequest>
    {
        public UserPromoteRequestValidator(WebDbContext dbContext) 
        {
            RuleFor(x => x.Id)
                .NotNull()
                    .WithMessage(UsersValidationCodes.UVC_016.GetDescription(String.Empty))
                .NotEmpty()
                    .WithMessage(UsersValidationCodes.UVC_016.GetDescription(String.Empty))
                .Custom((id, context) =>
                {
                    var user = dbContext.Users.FirstOrDefault(x => x.Id == id);
                    if (user == null)
                    {
                        context.AddFailure(UsersValidationCodes.UVC_016.GetDescription(String.Empty));
                    }
                });

            RuleFor(x => x.UserType)
                .NotEmpty()
                .Custom((type, context) =>
                {
                    if(!Enum.IsDefined(typeof(UserType), type))
                    {
                        context.AddFailure(UsersValidationCodes.UVC_015.GetDescription(String.Empty));
                    }
                });
        }
    }
}
