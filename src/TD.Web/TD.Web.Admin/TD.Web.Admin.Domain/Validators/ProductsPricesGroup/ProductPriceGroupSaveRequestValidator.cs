using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.ProductPriceGroup;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.ProductsPricesGroup;

public class ProductPriceGroupSaveRequestValidator
	: LSCoreValidatorBase<ProductPriceGroupSaveRequest>
{
	private readonly int _maximumNameLength = 32;
	private readonly int _minimumNameLength = 6;

	public ProductPriceGroupSaveRequestValidator(WebDbContext dbContext)
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.NotNull()
			.MinimumLength(_minimumNameLength)
			.MaximumLength(_maximumNameLength)
			.Custom(
				(name, context) =>
				{
					var entity = dbContext.ProductPriceGroups.FirstOrDefault(x => x.Name == name);
					if (entity != null)
					{
						context.AddFailure(
							ProductsPricesGroupValidationCodes.PPGVC_001.GetDescription()
						);
					}
				}
			);
	}
}
