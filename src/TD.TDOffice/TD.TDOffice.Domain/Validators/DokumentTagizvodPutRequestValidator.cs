using FluentValidation;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.TDOffice.Contracts.Enums.ValidationCodes;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;

namespace TD.TDOffice.Domain.Validators
{
    public class DokumentTagizvodPutRequestValidator : ValidatorBase<DokumentTagizvodPutRequest>
    {
        public DokumentTagizvodPutRequestValidator()
        {
            RuleFor(x => x.UnosPocetnoStanje)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Korisnik)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x)
                .Must(x => x.Id.HasValue && !x.BrojDokumentaIzvoda.HasValue ||
                        !x.Id.HasValue && x.BrojDokumentaIzvoda.HasValue)
                .WithMessage(ValidationCodes.DokumentTagIzvodValidationCodes.DTI001.GetDescription());
        }
    }
}
