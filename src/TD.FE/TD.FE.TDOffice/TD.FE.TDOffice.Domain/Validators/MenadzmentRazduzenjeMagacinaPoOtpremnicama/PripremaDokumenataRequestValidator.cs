using FluentValidation;
using LSCore.Contracts.Enums.ValidationCodes;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using TD.FE.TDOffice.Contracts.Requests.MenadzmentRazduzenjeMagacinaPoOtpremnicama;

namespace TD.FE.TDOffice.Domain.Validators.MenadzmentRazduzenjeMagacinaPoOtpremnicama
{
    public class PripremaDokumenataRequestValidator : LSCoreValidatorBase<PripremaDokumenataRequest>
    {
        public PripremaDokumenataRequestValidator()
        {
            RuleFor(x => x.MagacinId)
                .NotEmpty();

            RuleFor(x => x.VrDok)
                .NotNull();

            RuleFor(x => x.OdDatuma)
                .NotEmpty();

            RuleFor(x => x.DoDatuma)
                .NotEmpty();

            RuleFor(x => x)
                .Must(x => x.OdDatuma <= x.DoDatuma)
                .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_001.GetDescription(), nameof(PripremaDokumenataRequest.OdDatuma), nameof(PripremaDokumenataRequest.DoDatuma)));

            RuleFor(x => x.Namena)
                .NotEmpty();

            RuleFor(x => x.NacinPlacanja)
                .NotEmpty();
        }
    }
}
