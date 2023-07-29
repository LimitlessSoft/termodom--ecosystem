using FluentValidation;

namespace TD.Core.Domain.Validators
{
    public class ValidatorBase<TRequest> : AbstractValidator<TRequest>, Contracts.IValidators.IValidator<TRequest>
    {
        public ValidatorBase() : base()
        {

        }
    }
}
