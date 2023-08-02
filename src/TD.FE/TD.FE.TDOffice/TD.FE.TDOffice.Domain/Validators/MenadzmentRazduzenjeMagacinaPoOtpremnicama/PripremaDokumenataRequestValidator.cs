using FluentValidation;
using TD.Core.Contracts.Enums.ValidationCodes;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.FE.TDOffice.Contracts.Requests.MenadzmentRazduzenjeMagacinaPoOtpremnicama;

namespace TD.FE.TDOffice.Domain.Validators.MenadzmentRazduzenjeMagacinaPoOtpremnicama
{
    public class PripremaDokumenataRequestValidator : ValidatorBase<PripremaDokumenataRequest>
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
                .WithMessage(string.Format(CommonValidationCodes.COMM_001.GetDescription(), nameof(PripremaDokumenataRequest.OdDatuma), nameof(PripremaDokumenataRequest.DoDatuma)));

            RuleFor(x => x.Namena)
                .NotEmpty();

            RuleFor(x => x.NacinPlacanja)
                .NotEmpty();
        }
    }
}
