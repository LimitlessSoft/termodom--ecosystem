using FluentValidation;
using TD.Core.Domain.Validators;
using TD.Web.Admin.Contracts.Requests.Images;

namespace TD.Web.Admin.Domain.Validators.Images
{
    public class ImageGetRequestValidator : ValidatorBase<ImagesGetRequest>
    {
        private readonly int _maxQuality = 2160;
        public ImageGetRequestValidator()
        {
            RuleFor(x => x.Quality)
                .Must(x => x < _maxQuality);
        }
    }
}
