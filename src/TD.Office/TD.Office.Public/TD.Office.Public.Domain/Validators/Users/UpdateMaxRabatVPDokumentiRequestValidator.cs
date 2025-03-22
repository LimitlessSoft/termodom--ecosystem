using FluentValidation;
using LSCore.Validation.Domain;
using TD.Office.Public.Contracts;
using TD.Office.Public.Contracts.Requests.Users;

namespace TD.Office.Public.Domain.Validators.Users;

public class UpdateMaxRabatVPDokumentiRequestValidator
	: LSCoreValidatorBase<UpdateMaxRabatVPDokumentiRequest>
{
	public UpdateMaxRabatVPDokumentiRequestValidator()
	{
		RuleFor(x => x.MaxRabatVPDokumenti)
			.GreaterThanOrEqualTo(LegacyConstants.MinRabatVPDokumenti)
			.LessThanOrEqualTo(LegacyConstants.MaxRabatVPDokumenti);
	}
}
