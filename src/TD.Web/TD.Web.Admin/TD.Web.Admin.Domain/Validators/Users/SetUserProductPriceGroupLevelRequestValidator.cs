using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Web.Common.Contracts;
using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.Users;

public class SetUserProductPriceGroupLevelRequestValidator
	: LSCoreValidatorBase<SetUserProductPriceGroupLevelRequest>
{
	public SetUserProductPriceGroupLevelRequestValidator(IWebDbContextFactory dbContextFactory)
	{
		RuleFor(x => x.Id)
			.NotNull()
			.Must(z => dbContextFactory.Create<WebDbContext>().Users.Any(y => y.Id == z))
			.WithMessage(UsersValidationCodes.UVC_018.GetDescription());

		RuleFor(x => x.ProductPriceGroupId)
			.NotNull()
			.Must(z =>
				dbContextFactory.Create<WebDbContext>().ProductPriceGroups.Any(y => y.Id == z)
			)
			.WithMessage(UsersValidationCodes.UVC_019.GetDescription());

		RuleFor(x => x.Level)
			.NotNull()
			.Must(z => z >= 0 && z < LegacyConstants.NumberOfProductPriceGroupLevels);
	}
}
