using FluentValidation;
using TD.Core.Contracts.Http.Interfaces;

namespace TD.Core.Domain.Validators
{
    public static class Extensions
    {   
        public static bool IsRequestInvalid<TRequest, TResponse>(this TRequest request, TResponse response)
            where TResponse : IResponse
        {
            var validator = (IValidator<TRequest>?)Constants.Container?.TryGetInstance(typeof(IValidator<TRequest>));
            if (validator == null)
                return false;

            var validationResult = validator.Validate(request);

            if(!validationResult.IsValid)
            {
                response.Status = System.Net.HttpStatusCode.BadRequest;
                response.Errors = new List<string>(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            return !validationResult.IsValid;
        }
    }
}
