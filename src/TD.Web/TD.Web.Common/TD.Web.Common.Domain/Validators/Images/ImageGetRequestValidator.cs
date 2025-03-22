using FluentValidation;
using LSCore.Validation.Domain;
using TD.Web.Common.Contracts.Requests.Images;

namespace TD.Web.Common.Domain.Validators.Images;

public class ImageGetRequestValidator : LSCoreValidatorBase<ImagesGetRequest>
{
	private const int MaxQuality = 2160;

	public ImageGetRequestValidator()
	{
		RuleFor(x => x.Quality).Must(x => x < MaxQuality);
	}
}
