using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.Users;

public class UserPromoteRequestValidator : LSCoreValidatorBase<UserPromoteRequest>
{
	public UserPromoteRequestValidator(IWebDbContextFactory dbContextFactory)
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.Custom(
				(id, context) =>
				{
					var user = dbContextFactory
						.Create<WebDbContext>()
						.Users.FirstOrDefault(x => x.Id == id);
					if (user == null)
						context.AddFailure(UsersValidationCodes.UVC_016.GetDescription());
				}
			);

		RuleFor(x => x.Type)
			.NotEmpty()
			.Custom(
				(type, context) =>
				{
					if (!Enum.IsDefined(typeof(UserType), type))
						context.AddFailure(UsersValidationCodes.UVC_015.GetDescription());
				}
			);
	}
}
