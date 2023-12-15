using FluentValidation;
using LSCore.Domain.Validators;
using TD.Web.Common.Contracts.Requests.Images;

namespace TD.Web.Common.Domain.Validators.Images
{
    public class ImageGetRequestValidator : LSCoreValidatorBase<ImagesGetRequest>
    {
        private readonly int _maxQuality = 2160;
        public ImageGetRequestValidator()
        {
            RuleFor(x => x.Quality)
                .Must(x => x < _maxQuality);
        }
    }
}
