using FluentValidation;
using TD.Core.Contracts.Enums.ValidationCodes;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.Requests.Images;
using TD.Web.Contracts.Helpers.Images;
using TD.Web.Contracts.Enums.ValidationCodes;

namespace TD.Web.Domain.Validators.Images
{
    public class ImagesUploadRequestValidator : ValidatorBase<ImagesUploadRequest>
    {
        private int _altMaximumLength = 64;
        public ImagesUploadRequestValidator() 
        {
            RuleFor(x => x.Image)
                .NotNull()
                    .WithMessage(string.Format(CommonValidationCodes.COMM_002.GetDescription(String.Empty), nameof(ImagesUploadRequest.Image)))
                .NotEmpty()
                    .WithMessage(string.Format(CommonValidationCodes.COMM_002.GetDescription(String.Empty), nameof(ImagesUploadRequest.Image)))
                .Custom((image, context) =>
                {
                    var imgExt = image.FileName;
                    if(image.IsImageTypeNotValid())
                    {
                        context.AddFailure(ImagesValidationCodes.IVC_001.GetDescription(String.Empty));
                    }
                });

            RuleFor(x => x.AltText)
                .MaximumLength(_altMaximumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_003.GetDescription(String.Empty), nameof(ImagesUploadRequest.AltText), _altMaximumLength))
                .Custom((alt, context) =>
                {
                    if(alt.isAltValueNotValid())
                    {
                        context.AddFailure(ImagesValidationCodes.IVC_002.GetDescription(String.Empty));
                    }
                })
                .When(x => x.AltText != null);


        }
    }
}
