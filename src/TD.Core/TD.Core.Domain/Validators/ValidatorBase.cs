using FluentValidation;

namespace TD.Core.Domain.Validators
{
    public class ValidatorBase<TRequest> : AbstractValidator<TRequest>, Contracts.Validators.IValidator<TRequest>
    {
        public ValidatorBase()
        {

        }
    }
}
