using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Office.Public.Contracts.Enums.ValidationCodes;
using TD.Office.Public.Contracts.Requests.NalogZaPrevoz;

namespace TD.Office.Public.Domain.Validators.NalogZaPrevoz
{
	public class SaveNalogZaPrevozRequestValidator : LSCoreValidatorBase<SaveNalogZaPrevozRequest>
	{
		public SaveNalogZaPrevozRequestValidator()
		{
			RuleFor(x => x.Mobilni).NotEmpty();

			RuleFor(x => x.CenaPrevozaBezPdv).GreaterThan(0);

			RuleFor(x => x.MiNaplatiliKupcuBezPdv).GreaterThan(0);

			RuleFor(x => x.Address).NotEmpty();

			RuleFor(x => x.StoreId).GreaterThan(0);

			RuleFor(x => x.Prevoznik).NotEmpty();

			RuleFor(x => x)
				.Must(nalog => !string.IsNullOrEmpty(nalog.Note))
				.WithMessage(NalogZaPrevozValidationCodes.NZPVC_001.GetDescription())
				.When(x => x.VrDok == null);

			RuleFor(x => x.BrDok)
				.GreaterThan(0)
				.WithMessage(NalogZaPrevozValidationCodes.NZPVC_003.GetDescription())
				.When(nalog => nalog.VrDok != null);

			RuleFor(x => x.VrDok)
				.GreaterThan(0)
				.WithMessage(NalogZaPrevozValidationCodes.NZPVC_002.GetDescription())
				.When(nalog => nalog.VrDok != null);
		}
	}
}
