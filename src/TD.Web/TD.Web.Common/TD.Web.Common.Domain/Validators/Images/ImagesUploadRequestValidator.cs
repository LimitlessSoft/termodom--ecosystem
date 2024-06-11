using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Requests.Images;
using TD.Web.Common.Contracts.Helpers.Images;
using LSCore.Contracts.Enums.ValidationCodes;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using FluentValidation;

namespace TD.Web.Common.Domain.Validators.Images
{
    public class ImagesUploadRequestValidator : LSCoreValidatorBase<ImagesUploadRequest>
    {
        private const int AltMaximumLength = 64;

        public ImagesUploadRequestValidator() 
        {
            RuleFor(x => x.Image)
                .NotNull()
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_002.GetDescription()!, nameof(ImagesUploadRequest.Image)))
                .NotEmpty()
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_002.GetDescription()!, nameof(ImagesUploadRequest.Image)))
                .Custom((image, context) =>
                {
                    if(image.IsImageTypeNotValid())
                        context.AddFailure(ImagesValidationCodes.IVC_001.GetDescription());
                });

            RuleFor(x => x.AltText)
                .MaximumLength(AltMaximumLength)
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_003.GetDescription()!, nameof(ImagesUploadRequest.AltText), AltMaximumLength))
                .Custom((alt, context) =>
                {
                    if(alt.isAltValueNotValid())
                        context.AddFailure(ImagesValidationCodes.IVC_002.GetDescription());
                })
                .When(x => x.AltText != null);


        }
    }
}
