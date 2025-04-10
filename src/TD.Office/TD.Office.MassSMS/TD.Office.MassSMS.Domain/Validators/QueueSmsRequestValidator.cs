using FluentValidation;
using LSCore.Validation.Contracts;
using LSCore.Validation.Domain;
using TD.Office.MassSMS.Contracts.Constants;
using TD.Office.MassSMS.Contracts.Enums;
using TD.Office.MassSMS.Contracts.Enums.ValidationCodes;
using TD.Office.MassSMS.Contracts.Interfaces;
using TD.Office.MassSMS.Contracts.Requests;
using TD.Office.MassSMS.Repository.Repositories;

namespace TD.Office.MassSMS.Domain.Validators;

public class QueueSmsRequestValidator : LSCoreValidatorBase<QueueSmsRequest>
{
	public QueueSmsRequestValidator(
		IMassSMSDbContextFactory dbFactory,
		IPhoneValidatorSRB phoneValidatorSrb
	)
	{
		RuleFor(x => x.Message)
			.Cascade(CascadeMode.Stop)
			.NotEmpty()
			.WithMessage(SMSValidationCodes.SVC_003.GetValidationMessage())
			.MaximumLength(SMSConstants.MaxCharacters)
			.WithMessage(
				SMSValidationCodes.SVC_004.GetValidationMessage(SMSConstants.MaxCharacters)
			)
			.Must(x => x.All(char.IsAscii))
			.WithMessage(SMSValidationCodes.SVC_001.GetValidationMessage());

		RuleFor(x => x.PhoneNumber)
			.NotEmpty()
			.WithMessage(SMSValidationCodes.SVC_005.GetValidationMessage())
			.Must(x =>
			{
				try
				{
					phoneValidatorSrb.Format(x);
				}
				catch (ArgumentException e)
				{
					return false;
				}

				return true;
			})
			.WithMessage(SMSValidationCodes.SVC_006.GetValidationMessage());

		RuleFor(x => x)
			.Custom(
				(x, context) =>
				{
					var db = dbFactory.Create();
					var settingsRepository = new SettingRepository(db);
					var state = settingsRepository.GetGlobalState();
					if (state != GlobalState.Initial)
						context.AddFailure(SMSValidationCodes.SVC_002.GetValidationMessage(state));
				}
			)
			.When(x => x.Message != "Termodom"); // This is to bypass if validating with mass queue
	}
}
