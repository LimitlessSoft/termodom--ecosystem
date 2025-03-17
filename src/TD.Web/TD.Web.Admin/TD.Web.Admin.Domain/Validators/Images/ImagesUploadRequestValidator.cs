using FluentValidation;
using LSCore.Validation.Domain;
using TD.Web.Common.Contracts.Requests.Images;

namespace TD.Web.Common.Domain.Validators.Images
{
	public class ImagesUploadRequestValidator : LSCoreValidatorBase<ImagesUploadRequest>
	{
		private int _altMaximumLength = 64;

		public ImagesUploadRequestValidator()
		{
			RuleFor(x => x.Image)
				.NotNull()
				.NotEmpty()
				.Custom(
					(image, context) => {
						// var imgExt = image.FileName;
						// if(image.IsImageTypeNotValid())
						// {
						//     context.AddFailure(ImagesValidationCodes.IVC_001.GetDescription());
						// }
					}
				);

			RuleFor(x => x.AltText)
				.MaximumLength(_altMaximumLength)
				.Custom(
					(alt, context) => {
						// if(alt.isAltValueNotValid())
						// {
						//     context.AddFailure(ImagesValidationCodes.IVC_002.GetDescription());
						// }
					}
				)
				.When(x => x.AltText != null);
		}
	}
}
