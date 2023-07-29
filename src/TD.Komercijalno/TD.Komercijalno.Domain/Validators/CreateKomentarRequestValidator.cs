using FluentValidation;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.Komercijalno.Contracts.Enums.ValidationCodes;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Komentari;

namespace TD.Komercijalno.Domain.Validators
{
    public class CreateKomentarRequestValidator : ValidatorBase<CreateKomentarRequest>
    {
        public CreateKomentarRequestValidator() : base()
        {
            RuleFor(x => x.VrDok)
                .NotNull();

            RuleFor(x => x.BrDok)
                .NotNull();

            RuleFor(x => x)
                .Must(x =>
                    !string.IsNullOrWhiteSpace(x.Komentar) ||
                    !string.IsNullOrWhiteSpace(x.InterniKomentar) ||
                    !string.IsNullOrWhiteSpace(x.PrivatniKomentar))
                .WithMessage(string.Format(CreateKomentarRequestValidatorValidationCodes.CKRV_001.GetDescription(), nameof(CreateKomentarRequest.Komentar), nameof(CreateKomentarRequest.InterniKomentar), nameof(CreateKomentarRequest.PrivatniKomentar)));

            RuleFor(x => x)
                .Custom((request, context) =>
                {
                    // TODO: make this work
                    //var getKomentarResponse = komentarManager.Get(new GetKomentarRequest()
                    //{
                    //    VrDok = request.VrDok,
                    //    BrDok = request.BrDok
                    //});

                    //if (getKomentarResponse.Status != System.Net.HttpStatusCode.NoContent)
                    //    context.AddFailure(CreateKomentarRequestValidatorValidationCodes.CKRV_002.GetDescription());
                });
        }
    }
}
