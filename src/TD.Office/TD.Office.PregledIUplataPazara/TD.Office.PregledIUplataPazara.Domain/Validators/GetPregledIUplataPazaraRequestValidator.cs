using FluentValidation;
using LSCore.Validation.Contracts;
using LSCore.Validation.Domain;
using TD.Office.PregledIUplataPazara.Contracts.Requests;
using TD.Office.PregledIUplataPazara.Contracts.ValidationCodes;

namespace TD.Office.PregledIUplataPazara.Domain.Validators;

public class GetPregledIUplataPazaraRequestValidator
	: LSCoreValidatorBase<GetPregledIUplataPazaraRequest>
{
	public GetPregledIUplataPazaraRequestValidator()
	{
		RuleFor(x => x.OdDatumaInclusive)
			.NotEmpty()
			.WithMessage(PregledIUplataPazaraValiadtionCodes.PUPVC_001.GetValidationMessage());
		RuleFor(x => x.DoDatumaInclusive)
			.NotEmpty()
			.WithMessage(PregledIUplataPazaraValiadtionCodes.PUPVC_002.GetValidationMessage());
		RuleFor(x => x.Magacin)
			.NotEmpty()
			.WithMessage(PregledIUplataPazaraValiadtionCodes.PUPVC_003.GetValidationMessage());
		RuleFor(x => x.OdDatumaInclusive)
			.LessThanOrEqualTo(x => x.DoDatumaInclusive)
			.WithMessage(PregledIUplataPazaraValiadtionCodes.PUPVC_004.GetValidationMessage());
	}
}
