using FluentValidation;
using TD.Core.Domain.Validators;
using TD.Komercijalno.Contracts.Requests.Roba;

namespace TD.Komercijalno.Domain.Validators
{
    public class RobaCreateRequestValidator : ValidatorBase<RobaCreateRequest>
    {
        public RobaCreateRequestValidator()
        {
            RuleFor(x => x.KatBr)
                .NotEmpty()
                .MaximumLength(25);

            RuleFor(x => x.KatBrPro)
                .NotEmpty()
                .MaximumLength(25);

            RuleFor(x => x.Naziv)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.GrupaId)
                .NotEmpty()
                .MaximumLength(6);

            RuleFor(x => x.JM)
                .NotEmpty()
                .MaximumLength(3);

            RuleFor(x => x.TarifaId)
                .NotEmpty()
                .MaximumLength(3);
        }
    }
}
