using FluentValidation;
using LSCore.Domain.Validators;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;

namespace TD.TDOffice.Domain.Validators
{
    public class DokumentTagizvodPutRequestValidator : LSCoreValidatorBase<DokumentTagizvodPutRequest>
    {
        public DokumentTagizvodPutRequestValidator()
        {
            RuleFor(x => x.UnosPocetnoStanje)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Korisnik)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.BrojDokumentaIzvoda)
                .NotEmpty()
                .When(x => !x.Id.HasValue);
        }
    }
}
