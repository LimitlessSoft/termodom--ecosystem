using FluentValidation;
using TD.Core.Domain.Validators;
using TD.FE.TDOffice.Contracts.Requests.MenadzmentRazduzenjeMagacinaPoOtpremnicama;

namespace TD.FE.TDOffice.Domain.Validators.MenadzmentRazduzenjeMagacinaPoOtpremnicama
{
    public class RazduziMagacinRequestValidator : ValidatorBase<RazduziMagacinRequest>
    {
        public RazduziMagacinRequestValidator()
        {
            RuleFor(x => x.Izvor)
                .NotEmpty();

            RuleFor(x => x.DestinacijaBrDok)
                .NotEmpty()
                .When(x => !x.NoviDokument);

            RuleFor(x => x.DestinacijaMagacinId)
                .NotEmpty()
                .When(x => x.NoviDokument);

            RuleFor(x => x.DestinacijaNacinPlacanja)
                .NotEmpty()
                .When(x => x.NoviDokument);

            RuleFor(x => x.DestinacijaNamena)
                .NotEmpty()
                .When(x => x.NoviDokument);

            RuleFor(x => x.DestinacijaReferent)
                .NotEmpty()
                .When(x => x.NoviDokument);

            RuleFor(x => x.DestinacijaZaposleni)
                .NotEmpty()
                .When(x => x.NoviDokument);
        }
    }
}
