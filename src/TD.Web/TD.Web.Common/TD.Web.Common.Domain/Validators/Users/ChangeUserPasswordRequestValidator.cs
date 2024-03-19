using FluentValidation;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.IValidators;
using LSCore.Domain.Validators;
using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Helpers.Users;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Common.Repository;

namespace TD.Web.Common.Domain.Validators.Users
{
    public class ChangeUserPasswordRequestValidator : LSCoreValidatorBase<ChangeUserPasswordRequest>
    {
        public ChangeUserPasswordRequestValidator(WebDbContext dbContext)
        {
            RuleFor(request => request.Password).SetValidator(new UserPasswordValidator(dbContext));
        }
    }
}
