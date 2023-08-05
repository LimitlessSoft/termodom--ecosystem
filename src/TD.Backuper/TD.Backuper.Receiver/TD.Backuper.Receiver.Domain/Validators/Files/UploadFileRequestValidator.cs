using FluentValidation;
using TD.Backuper.Common.Contracts.Helpers;
using TD.Backuper.Receiver.Contracts.Requests.Files;
using TD.Core.Domain.Validators;

namespace TD.Backuper.Receiver.Domain.Validators.Files
{
    public class UploadFileRequestValidator : ValidatorBase<UploadRequest>
    {
        public UploadFileRequestValidator()
        {
            RuleFor(x => x.Key)
                .Must(x => KeyHelpers.VerifyKey(x))
                .WithMessage("Invalid key");
        }
    }
}
