using FluentValidation;
using TD.Backuper.Sender.Contracts.Dtos.Settings;
using TD.Backuper.Sender.Contracts.Enums.ValidationCodes;
using TD.Backuper.Sender.Contracts.Requests.Settings;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;

namespace TD.Backuper.Sender.Domain.Validators.Settings
{
    public class PutSettingRequestValidator : ValidatorBase<PutSettingRequest>
    {
        public PutSettingRequestValidator()
        {
            RuleFor(x => x.SettingsItems)
                .NotNull();

            RuleFor(x => x.SettingsItems)
                .Must(y => y.All(z => !string.IsNullOrWhiteSpace(z.Path)))
                    .WithMessage(string.Format(SettingsValidationCodes.SV001.GetDescription(), nameof(SettingPutRequestItemDto.Path)))
                .Must(y => y.All(z => z.IntervalMinutes > 0))
                    .WithErrorCode(string.Format(SettingsValidationCodes.SV002.GetDescription(), nameof(SettingPutRequestItemDto.IntervalMinutes)));
        }
    }
}
