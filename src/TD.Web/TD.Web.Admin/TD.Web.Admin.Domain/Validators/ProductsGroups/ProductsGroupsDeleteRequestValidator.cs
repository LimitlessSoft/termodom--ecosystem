using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.ProductsGroups;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.ProductsGroups;

public class ProductsGroupsDeleteRequestValidator : LSCoreValidatorBase<ProductsGroupsDeleteRequest>
{
	public ProductsGroupsDeleteRequestValidator(IWebDbContextFactory dbContextFactory)
	{
		RuleFor(x => x)
			.Must(x =>
				!dbContextFactory
					.Create<WebDbContext>()
					.Products.Any(z => z.Groups.Any(k => k.Id == x.Id))
			)
			.WithMessage(ProductsGroupsValidationCodes.PGVC_005.GetDescription());

		RuleFor(x => x.Id)
			.Must(x =>
				!dbContextFactory
					.Create<WebDbContext>()
					.ProductGroups.Any(z => z.ParentGroupId == x && z.IsActive)
			)
			.WithMessage(ProductsGroupsValidationCodes.PGVC_004.GetDescription());
	}
}
