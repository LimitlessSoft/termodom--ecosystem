using FluentValidation;
using LSCore.Domain.Validators;
using TD.Web.Public.Contracts.Requests.Calculator;

namespace TD.Web.Public.Domain.Validators.Calculator;

public class GetCalculatorItemsRequestValidator : LSCoreValidatorBase<GetCalculatorItemsRequest>
{
    public GetCalculatorItemsRequestValidator()
    {
        RuleFor(x => x.Type).NotNull();
    }
}
