using FluentValidation;
using LSCore.Validation.Domain;
using TD.Office.PregledIUplataPazara.Contracts.Requests;

namespace TD.Office.PregledIUplataPazara.Domain.Validators;

public class GetPregledIUplataPazaraRequestValidator : LSCoreValidatorBase<GetPregledIUplataPazaraRequest>
{
    public GetPregledIUplataPazaraRequestValidator()
    {
        RuleFor(x => x.OdDatumaInclusive)
            .NotEmpty();
        RuleFor(x => x.DoDatumaInclusive)
            .NotEmpty();
        RuleFor(x => x.OdDatumaInclusive)
            .LessThanOrEqualTo(x => x.DoDatumaInclusive);
    }
}