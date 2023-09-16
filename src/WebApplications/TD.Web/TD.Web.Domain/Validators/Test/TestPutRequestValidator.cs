using FluentValidation;
using TD.Core.Contracts.Enums.ValidationCodes;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.Requests.Test;

namespace TD.Web.Domain.Validators.Test
{
    public class TestPutRequestValidator : ValidatorBase<TestPutRequest>
    {
        private const int NameMinimumLenght = 1;
        private const int NameMaximumLength = 10;
        private const string NotAllowedCharacters = "!@#$%^&*().";
        public TestPutRequestValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.OrganizationName)
                .Must(x =>
                {
                    foreach (var c in x)
                        if (NotAllowedCharacters.Contains(c))
                            return false;

                    return true;
                })
                .Length(10);

            RuleFor(x => x.OrganizationName)
                .Must(x =>
                {
                    foreach (var c in x)
                        if (NotAllowedCharacters.Contains(c))
                            return false;

                    return true;
                });

        }
    }
}
