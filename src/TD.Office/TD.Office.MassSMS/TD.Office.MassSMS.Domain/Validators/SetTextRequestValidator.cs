using FluentValidation;
using LSCore.Validation.Contracts;
using LSCore.Validation.Domain;
using TD.Office.MassSMS.Contracts.Constants;
using TD.Office.MassSMS.Contracts.Enums.ValidationCodes;
using TD.Office.MassSMS.Contracts.Requests;

namespace TD.Office.MassSMS.Domain.Validators;

public class SetTextRequestValidator : LSCoreValidatorBase<SetTextRequest>
{
	public SetTextRequestValidator()
	{
		RuleFor(x => x.Text)
			.Cascade(CascadeMode.Stop)
			.NotEmpty()
			.WithMessage(SMSValidationCodes.SVC_003.GetValidationMessage())
			.MaximumLength(SMSConstants.MaxCharacters)
			.WithMessage(
				SMSValidationCodes.SVC_004.GetValidationMessage(SMSConstants.MaxCharacters)
			)
			.Must(x => x.All(char.IsAscii))
			.WithMessage(SMSValidationCodes.SVC_001.GetValidationMessage());
	}
}
