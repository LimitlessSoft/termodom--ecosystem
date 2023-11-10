using FluentValidation;
using LSCore.Domain.Validators;
using TD.Komercijalno.Contracts.Requests.Procedure;

namespace TD.Komercijalno.Domain.Validators
{
    public class ProceduraGetProdajnaCenaNaDanRequestValidator : LSCoreValidatorBase<ProceduraGetProdajnaCenaNaDanRequest>
    {
        public ProceduraGetProdajnaCenaNaDanRequestValidator()
        {
            RuleFor(x => x)
                .Must(x =>
                    (x.ZaobidjiVrDok == null && x.ZaobidjiBrDok == null) ||
                    (x.ZaobidjiVrDok != null && x.ZaobidjiBrDok != null));
        }
    }
}
