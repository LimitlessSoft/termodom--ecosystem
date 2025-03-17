using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Helpers.Units;
using TD.Web.Admin.Contracts.Requests.Units;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.Units;

public class UnitsSaveRequestValidator : LSCoreValidatorBase<UnitSaveRequest>
{
	private const int NameMaximumLength = 32;
	private const int NameMinimumLength = 1;

	public UnitsSaveRequestValidator(WebDbContext dbContext)
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(NameMaximumLength)
			.MinimumLength(NameMinimumLength)
			.Custom(
				(name, context) =>
				{
					if (name.IsNameNotValid())
						context.AddFailure(
							string.Format(
								UnitsValidationCodes.UVC_002.GetDescription(),
								nameof(UnitSaveRequest.Name)
							)
						);

					var unit = dbContext.Units.FirstOrDefault(x => x.Name == name.NormalizeName());
					if (unit != null)
						context.AddFailure(
							string.Format(
								UnitsValidationCodes.UVC_001.GetDescription(),
								nameof(UnitSaveRequest.Name)
							)
						);
				}
			);
	}
}
