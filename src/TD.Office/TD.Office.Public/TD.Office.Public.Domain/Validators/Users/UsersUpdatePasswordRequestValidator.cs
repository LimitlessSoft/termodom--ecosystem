using FluentValidation;
using LSCore.Domain.Validators;
using TD.Office.Public.Contracts.Requests.Users;

namespace TD.Office.Public.Domain.Validators.Users;

public class UsersUpdatePasswordRequestValidator : LSCoreValidatorBase<UsersUpdatePasswordRequest>
{
    public UsersUpdatePasswordRequestValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}