using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.ProductsGroups;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.ProductsGroups;

public class ProductsGroupsSaveRequestValidator : LSCoreValidatorBase<ProductsGroupsSaveRequest>
{
	private readonly int _nameMaximumLength = 32;
	private readonly int _nameMinimumLength = 5;

	public ProductsGroupsSaveRequestValidator(IWebDbContextFactory dbContextFactory)
	{
		RuleFor(x => x)
			.Custom(
				(request, context) =>
				{
					var group = dbContextFactory
						.Create<WebDbContext>()
						.ProductGroups.FirstOrDefault(x =>
							x.Name.ToLower() == request.Name.ToLower() && x.IsActive
						);

					if (
						request.Id.HasValue
						&& request.ParentGroupId.HasValue
						&& request.Id == request.ParentGroupId
					)
						context.AddFailure(ProductsGroupsValidationCodes.PGVC_003.GetDescription());

					if (group != null && group.Id != request.Id)
						context.AddFailure(ProductsGroupsValidationCodes.PGVC_002.GetDescription());
				}
			);

		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(_nameMaximumLength)
			.MinimumLength(_nameMinimumLength);

		RuleFor(x => x.ParentGroupId)
			.Must(x =>
				dbContextFactory
					.Create<WebDbContext>()
					.ProductGroups.Any(z => z.Id == x && z.IsActive)
			)
			.WithMessage(ProductsGroupsValidationCodes.PGVC_001.GetDescription())
			.When(x => x.ParentGroupId.HasValue);
	}
}
