using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.Units;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.Units;

public class UnitDeleteRequestValidator : LSCoreValidatorBase<UnitDeleteRequest>
{
	public UnitDeleteRequestValidator(IWebDbContextFactory dbContextFactory)
	{
		RuleFor(x => x)
			.Must(
				(request) =>
				{
					return !dbContextFactory
						.Create<WebDbContext>()
						.Products.Include(x => x.Unit)
						.Any(x => x.Id == request.Id);
				}
			)
			.WithMessage(UnitsValidationCodes.UVC_003.GetDescription());
	}
}
