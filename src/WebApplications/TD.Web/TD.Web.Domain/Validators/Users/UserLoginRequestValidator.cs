using FluentValidation;
using TD.Core.Contracts.Extensions;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.Enums.ValidationCodes;
using TD.Web.Contracts.Requests.Users;
using TD.Web.Repository;

namespace TD.Web.Domain.Validators.Users
{
    public class UserLoginRequestValidator : ValidatorBase<UserLoginRequest>
    {
        public UserLoginRequestValidator(WebDbContext dbContext)
        {
            RuleFor(x => x)
                .Custom((request, context) =>
                {
                    var user = dbContext.Users.FirstOrDefault(x => x.Username.ToUpper() == request.Username.ToUpper());

                    if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password))
                    {
                        context.AddFailure(UsersValidationCodes.UVC_006.GetDescription(String.Empty));
                        return;
                    }

                    if(user.ProcessingDate == null)
                    {
                        context.AddFailure(UsersValidationCodes.UVC_017.GetDescription(String.Empty));
                        return;
                    }
                });
        }
    }
}
