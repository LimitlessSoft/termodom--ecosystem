using TD.Office.Public.Contracts.Requests.Users;
using TD.Office.Common.Repository;
using LSCore.Domain.Validators;
using FluentValidation;
using LSCore.Contracts.Extensions;
using TD.Office.Common.Contracts.Enums.ValidationCodes;

namespace TD.Office.Public.Domain.Validators.Users;

public class UsersUpdatePermissionRequestValidator : LSCoreValidatorBase<UsersUpdatePermissionRequest>
{
    public UsersUpdatePermissionRequestValidator(OfficeDbContext dbContext)
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Permission)
            .NotEmpty();

        RuleFor(x => x.IsGranted)
            .NotNull();

        RuleFor(x => x)
            .Must(x => dbContext.Users.Any(u => u.Id == x.Id && u.IsActive))
                .WithMessage(UsersValidationCodes.UVC_002.GetDescription());
    }
}