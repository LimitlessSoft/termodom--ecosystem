using FluentValidation;
using LSCore.Domain.Validators;
using TD.Komercijalno.Contracts.Requests.Stavke;

namespace TD.Komercijalno.Domain.Validators
{
    public class StavkaCreateRequestValidator : LSCoreValidatorBase<StavkaCreateRequest>
    {
        public StavkaCreateRequestValidator()
        {
            RuleFor(x => x.VrDok)
                .NotEmpty();

            RuleFor(x => x.BrDok)
                .NotEmpty();

            RuleFor(x => x.RobaId)
                .NotEmpty();

            RuleFor(x => x.Kolicina)
                .NotNull();

            RuleFor(x => x.Rabat)
                .NotNull();
        }
    }
}
