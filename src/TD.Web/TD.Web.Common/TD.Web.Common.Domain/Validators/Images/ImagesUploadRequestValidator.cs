using FluentValidation;
using TD.Web.Common.Contracts.Requests.Images;
using TD.Web.Common.Contracts.Helpers.Images;
using LSCore.Domain.Validators;
using LSCore.Contracts.Enums.ValidationCodes;
using LSCore.Contracts.Extensions;
using TD.Web.Common.Contracts.Enums.ValidationCodes;

namespace TD.Web.Common.Domain.Validators.Images
{
    public class ImagesUploadRequestValidator : LSCoreValidatorBase<ImagesUploadRequest>
    {
        private int _altMaximumLength = 64;
        public ImagesUploadRequestValidator() 
        {
            RuleFor(x => x.Image)
            .NotNull()
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_002.GetDescription(), nameof(ImagesUploadRequest.Image)))
                .NotEmpty()
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_002.GetDescription(), nameof(ImagesUploadRequest.Image)))
                .Custom((image, context) =>
                {
                    var imgExt = image.FileName;
                    if(image.IsImageTypeNotValid())
                    {
                        context.AddFailure(ImagesValidationCodes.IVC_001.GetDescription());
                    }
                });

            RuleFor(x => x.AltText)
                .MaximumLength(_altMaximumLength)
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_003.GetDescription(), nameof(ImagesUploadRequest.AltText), _altMaximumLength))
                .Custom((alt, context) =>
                {
                    if(alt.isAltValueNotValid())
                    {
                        context.AddFailure(ImagesValidationCodes.IVC_002.GetDescription());
                    }
                })
                .When(x => x.AltText != null);


        }
    }
}
