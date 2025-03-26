using FluentValidation;
using LSCore.Validation.Domain;
using TD.Office.Public.Contracts.Requests.Users;

namespace TD.Office.Public.Domain.Validators.Users;

public class UsersUpdatePermissionRequestValidator
	: LSCoreValidatorBase<UsersUpdatePermissionRequest>
{
	public UsersUpdatePermissionRequestValidator()
	{
		RuleFor(x => x.Id).NotEmpty();

		RuleFor(x => x.Permission).NotEmpty();

		RuleFor(x => x.IsGranted).NotNull();
	}
}
