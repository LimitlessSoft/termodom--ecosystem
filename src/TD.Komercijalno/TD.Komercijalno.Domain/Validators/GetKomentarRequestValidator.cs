using FluentValidation;
using TD.Core.Domain.Validators;
using TD.Komercijalno.Contracts.Requests.Komentari;

namespace TD.Komercijalno.Domain.Validators
{
    public class GetKomentarRequestValidator : ValidatorBase<GetKomentarRequest>
    {
        public GetKomentarRequestValidator()
        {
            RuleFor(x => x.VrDok)
                .NotNull();

            RuleFor(x => x.BrDok)
                .NotEmpty();
        }
    }
}
