using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Web.Common.Contracts;
using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.Users
{
	public class SetUserProductPriceGroupLevelRequestValidator
		: LSCoreValidatorBase<SetUserProductPriceGroupLevelRequest>
	{
		public SetUserProductPriceGroupLevelRequestValidator(WebDbContext dbContext)
		{
			RuleFor(x => x.Id)
				.NotNull()
				.Must(z => dbContext.Users.Any(y => y.Id == z))
				.WithMessage(UsersValidationCodes.UVC_018.GetDescription());

			RuleFor(x => x.ProductPriceGroupId)
				.NotNull()
				.Must(z => dbContext.ProductPriceGroups.Any(y => y.Id == z))
				.WithMessage(UsersValidationCodes.UVC_019.GetDescription());

			RuleFor(x => x.Level)
				.NotNull()
				.Must(z => z >= 0 && z < LegacyConstants.NumberOfProductPriceGroupLevels);
		}
	}
}
