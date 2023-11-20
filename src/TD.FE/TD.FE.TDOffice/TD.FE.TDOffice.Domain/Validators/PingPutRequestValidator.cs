using FluentValidation;
using LSCore.Domain.Validators;
using TD.FE.TDOffice.Contracts.Requests.Ping;

namespace TD.FE.TDOffice.Domain.Validators
{
    public class PingPutRequestValidator : LSCoreValidatorBase<PingPutRequest>
    {
        public PingPutRequestValidator()
        {
            RuleFor(x => x.Value1)
                .NotEmpty()
                .WithMessage("Error");
        }
    }
}
