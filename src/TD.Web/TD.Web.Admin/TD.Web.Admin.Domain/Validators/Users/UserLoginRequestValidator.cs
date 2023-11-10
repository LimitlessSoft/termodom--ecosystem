using FluentValidation;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.Users;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.Users
{
    public class UserLoginRequestValidator : LSCoreValidatorBase<UserLoginRequest>
    {
        public UserLoginRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x)
                .Custom((request, context) =>
                {
                    var user = dbContext.Users.FirstOrDefault(x => x.Username.ToUpper() == request.Username.ToUpper());

                    if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password))
                    {
                        context.AddFailure(UsersValidationCodes.UVC_006.GetDescription());
                        return;
                    }

                    if(user.ProcessingDate == null)
                    {
                        context.AddFailure(UsersValidationCodes.UVC_017.GetDescription());
                        return;
                    }
                });
        }
    }
}
