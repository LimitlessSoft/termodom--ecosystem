using FluentValidation;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.Users;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.Users
{
    public class UserPromoteRequestValidator : ValidatorBase<UserPromoteRequest>
    {
        public UserPromoteRequestValidator(WebDbContext dbContext) 
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Custom((id, context) =>
                {
                    var user = dbContext.Users.FirstOrDefault(x => x.Id == id);
                    if (user == null)
                        context.AddFailure(UsersValidationCodes.UVC_016.GetDescription(String.Empty));
                });

            RuleFor(x => x.Type)
                .NotEmpty()
                .Custom((type, context) =>
                {
                    if(!Enum.IsDefined(typeof(UserType), type))
                        context.AddFailure(UsersValidationCodes.UVC_015.GetDescription(String.Empty));
                });
        }
    }
}
